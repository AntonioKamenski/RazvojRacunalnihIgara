using UnityEngine;

public class ArcherAttack : PlayerAttackBase
{
    [Header("Archer — Arrow Settings")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float arrowSpeed = 12f;

    private void Reset()
    {
        attackCooldown = 0.8f;
        attackWindupDelay = 0.2f;
        baseDamage = 18f;
        element = ElementType.Lightning;
        causesBleed = false;
        bleedBuildup = 22f;
    }

    public void SetArrowPrefab(GameObject prefab) => arrowPrefab = prefab;

    protected override void OnAttackSFX() => AudioManager.Instance?.PlayPlayerBowShot();

    protected override void Attack(EnemyBase target)
    {
        if (arrowPrefab == null) return;

        Vector2 direction = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;

        var proj = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        var script = proj.GetComponent<PlayerProjectile>();
        if (script != null)
            script.Init(direction, arrowSpeed, BuildDamage());
    }
}
