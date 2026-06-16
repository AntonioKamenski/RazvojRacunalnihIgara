using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBase : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float borderPaddingX = 1f;
    [SerializeField] private float borderPaddingY = 1f;

    [Header("Stats")]
    [SerializeField] protected float maxHP = 100f;
    [SerializeField] protected float defense = 0f;

    [Header("Element")]
    [SerializeField] protected ElementType bonusElement = ElementType.None;
    [SerializeField] protected float bonusElementMultiplier = 1.25f;

    [Header("UI")]
    [SerializeField] private Slider healthBar;

    protected float currentHP;
    protected bool isDead = false;

    // Passive ability hooks — set by PassiveAbilityHandler
    public System.Func<DamageInfo, DamageInfo> ModifyIncomingDamage;
    public System.Func<bool> ShouldDodge;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveInput;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        spriteRenderer = GetComponent<SpriteRenderer>();

        var diff = GameManager.Instance?.ActiveDifficulty;
        if (diff != null) maxHP *= diff.playerHPMultiplier;

        currentHP = maxHP;
        RefreshHealthBar();
    }

    private void Update()
    {
        ReadMovementInput();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
        ClampToBorders();
    }

    private void ReadMovementInput()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        float x = (kb.dKey.isPressed || kb.rightArrowKey.isPressed ? 1f : 0f)
                - (kb.aKey.isPressed || kb.leftArrowKey.isPressed  ? 1f : 0f);
        float y = (kb.wKey.isPressed || kb.upArrowKey.isPressed    ? 1f : 0f)
                - (kb.sKey.isPressed || kb.downArrowKey.isPressed   ? 1f : 0f);

        moveInput = new Vector2(x, y).normalized;

        if (spriteRenderer != null && x != 0)
            spriteRenderer.flipX = x < 0;
    }

    private void ClampToBorders()
    {
        if (MapBorders.Instance == null) return;

        Vector2 pos = rb.position;
        pos.x = Mathf.Clamp(pos.x, MapBorders.Instance.minX + borderPaddingX, MapBorders.Instance.maxX - borderPaddingX);
        pos.y = Mathf.Clamp(pos.y, MapBorders.Instance.minY + borderPaddingY, MapBorders.Instance.maxY - borderPaddingY);
        rb.position = pos;
    }

    public virtual void TakeDamage(DamageInfo info)
    {
        if (isDead) return;

        if (ShouldDodge != null && ShouldDodge())
            return;

        if (ModifyIncomingDamage != null)
            info = ModifyIncomingDamage(info);

        float amount = Mathf.Max(0f, info.Amount - defense);
        currentHP -= amount;
        RefreshHealthBar();
        AudioManager.Instance?.PlayPlayerHit();

        if (currentHP <= 0f)
            Die();
    }

    public void TakeDamage(float flatAmount)
    {
        TakeDamage(DamageInfo.Pure(flatAmount));
    }

    protected virtual void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        AudioManager.Instance?.PlayPlayerDeath();
        if (GameEndManager.Instance != null)
            GameEndManager.Instance.ShowDeath();
        else
            GameManager.Instance?.LoadMainMenu();
    }

    private void RefreshHealthBar()
    {
        if (healthBar == null) return;
        healthBar.value = Mathf.Clamp01(currentHP / maxHP);
    }

    public DamageInfo ApplyElementBonus(DamageInfo info)
    {
        if (bonusElement != ElementType.None && info.Element == bonusElement)
            info.Amount *= bonusElementMultiplier;
        return info;
    }

    public void SetBonusElement(ElementType element) => bonusElement = element;
    public void SetMoveSpeed(float speed) => moveSpeed = speed;
    public void AddMaxHP(float amount) { maxHP += amount; currentHP += amount; RefreshHealthBar(); }
    public void AddDefense(float amount) => defense += amount;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;
    public float MoveSpeed => moveSpeed;
    public ElementType BonusElement => bonusElement;
    public bool IsDead => isDead;
    public bool IsMoving => moveInput.sqrMagnitude > 0.01f;
}
