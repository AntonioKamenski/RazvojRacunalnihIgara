using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BleedHandler))]
public class EnemyBase : MonoBehaviour, IBleedable
{
    [Header("Stats")]
    [SerializeField] protected float maxHP = 100f;
    [SerializeField] protected float moveSpeed = 3f;
    [SerializeField] protected float contactDamage = 10f;
    [SerializeField] protected float attackCooldown = 1f;
    [SerializeField] protected int xpReward = 10;

    [Header("Border Clamp")]
    [SerializeField] private float borderPaddingX = 0.5f;
    [SerializeField] private float borderPaddingY = 0.5f;

    [Header("Element")]
    [SerializeField] protected ElementType weakElement = ElementType.Fire;
    [SerializeField] protected float weaknessMultiplier = 1.5f;
    [SerializeField] protected ElementType resistElement = ElementType.None;
    [SerializeField] protected float resistanceMultiplier = 0.5f;

    [Header("HP Bar (assign child Slider)")]
    [SerializeField] protected Slider healthBar;

    protected float currentHP;
    protected Rigidbody2D rb;
    protected BleedHandler bleedHandler;
    protected bool isDead = false;
    private float baseSpeed = -1f;
    private Coroutine slowCoroutine;

    public event Action<EnemyBase> OnDeath;

    protected virtual void Awake()
    {
        var diff = GameManager.Instance?.ActiveDifficulty;
        if (diff != null)
        {
            maxHP         *= diff.enemyHPMultiplier;
            contactDamage *= diff.enemyDamageMultiplier;
        }

        currentHP = maxHP;

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        bleedHandler = GetComponent<BleedHandler>();

        RefreshHealthBar();
    }

    public virtual void TakeDamage(DamageInfo info)
    {
        if (isDead) return;

        float amount = info.Amount;

        if (info.Element != ElementType.None)
        {
            if (info.Element == weakElement)
                amount *= weaknessMultiplier;
            else if (info.Element == resistElement)
                amount *= resistanceMultiplier;
        }

        currentHP -= amount;
        RefreshHealthBar();

        GameStatsTracker.Instance?.AddDamage(amount);
        DamagePopup.Spawn((Vector2)transform.position + Vector2.up * 0.5f, amount, info.Element);
        AudioManager.Instance?.PlayEnemyHit();

        if (info.CausesBleed)
            bleedHandler.AddBuildup(info.BleedBuildup);

        if (currentHP <= 0f)
            Die();
    }

    public void TakeBleedDamage(float damage)
    {
        if (isDead) return;
        currentHP -= damage;
        RefreshHealthBar();
        GameStatsTracker.Instance?.AddDamage(damage);
        if (currentHP <= 0f)
            Die();
    }

    protected virtual void Die()
    {
        isDead = true;
        AudioManager.Instance?.PlayEnemyDeath();
        GameStatsTracker.Instance?.AddKill();
        if (xpReward > 0 && GameStatsTracker.XPPickupPrefab != null)
        {
            var pickup = Instantiate(GameStatsTracker.XPPickupPrefab, transform.position, Quaternion.identity);
            pickup.GetComponent<XPPickup>()?.Init(xpReward);
        }
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }

    private void RefreshHealthBar()
    {
        if (healthBar == null) return;
        healthBar.value = Mathf.Clamp01(currentHP / maxHP);
    }

    public void ApplySlow(float speedFactor, float duration)
    {
        if (baseSpeed < 0f) baseSpeed = moveSpeed;
        if (slowCoroutine != null) StopCoroutine(slowCoroutine);
        slowCoroutine = StartCoroutine(SlowCoroutine(speedFactor, duration));
    }

    public void ClampToBorders()
    {
        if (MapBorders.Instance == null) return;

        const float minimumBorderPadding = 2f;

        Vector2 pos = rb.position;
        float paddingX = Mathf.Max(borderPaddingX, minimumBorderPadding);
        float paddingY = Mathf.Max(borderPaddingY, minimumBorderPadding);
        pos.x = Mathf.Clamp(pos.x, MapBorders.Instance.minX + paddingX, MapBorders.Instance.maxX - paddingX);
        pos.y = Mathf.Clamp(pos.y, MapBorders.Instance.minY + paddingY, MapBorders.Instance.maxY - paddingY);
        rb.position = pos;
    }

    private IEnumerator SlowCoroutine(float speedFactor, float duration)
    {
        moveSpeed = baseSpeed * speedFactor;
        yield return new WaitForSeconds(duration);
        moveSpeed = baseSpeed;
        slowCoroutine = null;
    }

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;
    public float ContactDamage => contactDamage;
    public float MoveSpeed => moveSpeed;
    public float AttackCooldown => attackCooldown;
    public bool IsDead => isDead;
    public ElementType WeakElement => weakElement;
}
