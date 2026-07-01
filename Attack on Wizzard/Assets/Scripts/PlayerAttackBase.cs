using System.Collections;
using UnityEngine;

public abstract class PlayerAttackBase : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] protected float attackCooldown = 1f;
    [SerializeField] protected float attackWindupDelay = 1f;
    [SerializeField] protected float baseDamage = 20f;
    [SerializeField] protected ElementType element = ElementType.None;
    [SerializeField] protected bool causesBleed = true;
    [SerializeField] protected float bleedBuildup = 25f;

    protected float cooldownTimer = 0f;
    protected PlayerBase playerBase;
    private CharacterAnimator characterAnimator;

    protected virtual void Awake()
    {
        playerBase = GetComponent<PlayerBase>();
        characterAnimator = GetComponent<CharacterAnimator>();
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            var target = FindNearestEnemy();
            if (target != null)
            {
                cooldownTimer = attackCooldown;
                StartCoroutine(AttackAfterDelay(target));
            }
        }
    }

    private IEnumerator AttackAfterDelay(EnemyBase target)
    {
        if (attackWindupDelay > 0f)
            yield return new WaitForSeconds(attackWindupDelay);

        if (target != null && !target.IsDead)
        {
            characterAnimator?.TriggerAttack();
            OnAttackSFX();
            Attack(target);
        }
    }

    protected abstract void Attack(EnemyBase target);

    protected EnemyBase FindNearestEnemy()
    {
        var enemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);
        EnemyBase nearest = null;
        float nearestDist = float.MaxValue;

        foreach (var e in enemies)
        {
            if (e.IsDead) continue;
            float dist = Vector2.Distance(transform.position, e.transform.position);
            if (dist < nearestDist) { nearestDist = dist; nearest = e; }
        }

        return nearest;
    }

    protected DamageInfo BuildDamage()
    {
        var info = new DamageInfo(baseDamage, element, causesBleed, bleedBuildup);
        return playerBase != null ? playerBase.ApplyElementBonus(info) : info;
    }

    protected virtual void OnAttackSFX() { }

    public void ModifyCooldown(float multiplier) => attackCooldown = Mathf.Max(0.1f, attackCooldown * multiplier);
    public void ModifyDamage(float multiplier)   => baseDamage = Mathf.Max(1f, baseDamage * multiplier);
    public float AttackCooldown => attackCooldown;
}
