using UnityEngine;

public class PassiveAbilityHandler : MonoBehaviour
{
    [Header("Projectile Prefabs")]
    [SerializeField] private GameObject orbPrefab;
    [SerializeField] private GameObject arrowPrefab;

    [Header("Archer Swiftness")]
    [SerializeField] private float swiftnessGainPerSecond = 0.1f;
    [SerializeField] private float swiftnessMax = 0.4f;

    private PlayerBase playerBase;
    private PlayerAttackBase attackScript;
    private float swiftnessBonus = 0f;
    private float baseAttackCooldown = 0f;

    private void Awake()
    {
        playerBase = GetComponent<PlayerBase>();
        ApplyClass(GameManager.Instance.SelectedClass);
    }

    private void ApplyClass(GameManager.PlayerClassType classType)
    {
        playerBase.SetBonusElement(ElementTypeForClass(classType));

        switch (classType)
        {
            case GameManager.PlayerClassType.Warrior:
                attackScript = gameObject.AddComponent<WarriorAttack>();
                playerBase.ModifyIncomingDamage = ApplyFortify;
                break;

            case GameManager.PlayerClassType.Rogue:
                attackScript = gameObject.AddComponent<RogueAttack>();
                playerBase.ShouldDodge = RollEvasion;
                break;

            case GameManager.PlayerClassType.Mage:
                var mage = gameObject.AddComponent<MageAttack>();
                mage.SetOrbPrefab(orbPrefab);
                attackScript = mage;
                break;

            case GameManager.PlayerClassType.Archer:
                var archer = gameObject.AddComponent<ArcherAttack>();
                archer.SetArrowPrefab(arrowPrefab);
                attackScript = archer;
                break;
        }

        if (attackScript != null)
            baseAttackCooldown = attackScript.AttackCooldown;
    }

    private void Update()
    {
        if (GameManager.Instance.SelectedClass != GameManager.PlayerClassType.Archer) return;
        if (attackScript == null) return;

        if (playerBase.IsMoving)
            swiftnessBonus = Mathf.Min(swiftnessMax, swiftnessBonus + swiftnessGainPerSecond * Time.deltaTime);
        else
            swiftnessBonus = 0f;
    }

    // Warrior passive — Fortify: 15% less damage when below 50% HP
    private DamageInfo ApplyFortify(DamageInfo info)
    {
        if (playerBase.CurrentHP / playerBase.MaxHP < 0.5f)
            info.Amount *= 0.85f;
        return info;
    }

    // Rogue passive — Evasion: 15% dodge chance
    private bool RollEvasion() => Random.value < 0.15f;

    private static ElementType ElementTypeForClass(GameManager.PlayerClassType classType)
    {
        return classType switch
        {
            GameManager.PlayerClassType.Warrior => ElementType.Fire,
            GameManager.PlayerClassType.Rogue   => ElementType.Shadow,
            GameManager.PlayerClassType.Mage    => ElementType.Ice,
            GameManager.PlayerClassType.Archer  => ElementType.Lightning,
            _                                   => ElementType.None
        };
    }
}
