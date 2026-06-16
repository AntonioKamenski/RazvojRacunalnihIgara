using UnityEngine;

public class MageAttack : PlayerAttackBase
{
    [Header("Mage — Orb Settings")]
    [SerializeField] private GameObject orbPrefab;
    [SerializeField] private float orbSpeed = 5f;
    [SerializeField] private float homingStrength = 4f;

    private void Reset()
    {
        attackCooldown = 1.6f;
        baseDamage = 32f;
        element = ElementType.Ice;
        causesBleed = false;
        bleedBuildup = 0f;
    }

    public void SetOrbPrefab(GameObject prefab) => orbPrefab = prefab;

    protected override void OnAttackSFX() => AudioManager.Instance?.PlayPlayerSpell();

    protected override void Attack(EnemyBase target)
    {
        if (orbPrefab == null) return;

        Vector2 direction = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;

        var proj = Instantiate(orbPrefab, transform.position, Quaternion.identity);
        var script = proj.GetComponent<PlayerProjectile>();
        if (script != null)
            script.Init(direction, orbSpeed, BuildDamage(), target.transform, homingStrength);
    }
}
