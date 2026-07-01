using UnityEngine;
using System.Collections;

public class RogueAttack : PlayerAttackBase
{
    [Header("Rogue — Dual Slash Settings")]
    [SerializeField] private float slashRange = 3f;
    [SerializeField] private float secondSlashDelay = 0.12f;

    private void Reset()
    {
        attackCooldown = 0.65f;
        baseDamage = 13f;
        element = ElementType.Shadow;
        causesBleed = true;
        bleedBuildup = 35f;
    }

    protected override void Attack(EnemyBase target)
    {
        StartCoroutine(DualSlash(target));
    }

    private IEnumerator DualSlash(EnemyBase target)
    {
        var damage = BuildDamage();
        damage.Amount *= 0.6f;

        if (!target.IsDead && InRange(target))
            target.TakeDamage(damage);

        yield return new WaitForSeconds(secondSlashDelay);

        if (target != null && !target.IsDead && InRange(target))
            target.TakeDamage(damage);
    }

    private bool InRange(EnemyBase e) =>
        Vector2.Distance(transform.position, e.transform.position) <= slashRange;

    protected override void OnAttackSFX() => AudioManager.Instance?.PlayPlayerMelee();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.5f, 0f, 0.5f, 0.4f);
        Gizmos.DrawWireSphere(transform.position, slashRange);
    }
}
