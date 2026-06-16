using UnityEngine;

public class WarriorAttack : PlayerAttackBase
{
    [Header("Warrior — Swing Settings")]
    [SerializeField] private float swingRange = 2f;

    private void Reset()
    {
        attackCooldown = 1.2f;
        baseDamage = 28f;
        element = ElementType.Fire;
        causesBleed = true;
        bleedBuildup = 30f;
    }

    protected override void Attack(EnemyBase target)
    {
        var damage = BuildDamage();

        foreach (var e in FindObjectsByType<EnemyBase>(FindObjectsSortMode.None))
        {
            if (!e.IsDead && Vector2.Distance(transform.position, e.transform.position) <= swingRange)
                e.TakeDamage(damage);
        }
    }

    protected override void OnAttackSFX() => AudioManager.Instance?.PlayPlayerMelee();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.4f, 0f, 0.4f);
        Gizmos.DrawWireSphere(transform.position, swingRange);
    }
}
