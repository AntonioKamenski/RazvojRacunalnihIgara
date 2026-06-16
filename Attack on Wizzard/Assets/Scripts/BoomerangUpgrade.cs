using UnityEngine;

public class BoomerangUpgrade : ActiveUpgradeBase
{
    private GameObject boomerangPrefab;
    private float cooldown = 4f;
    private float timer = 0f;
    private float damage = 22f;
    private float speed = 7f;
    private float range = 6f;

    public void Init(GameObject prefab) => boomerangPrefab = prefab;

    protected override void OnLevelUp()
    {
        damage += 10f;
        range += 1f;
        cooldown = Mathf.Max(1.5f, cooldown - 0.7f);
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;

        var target = FindNearestEnemy(transform.position);
        if (target == null) return;

        Fire(target);
        timer = cooldown;
    }

    private void Fire(EnemyBase target)
    {
        if (boomerangPrefab == null) return;

        Vector2 dir = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
        var proj = Instantiate(boomerangPrefab, transform.position, Quaternion.identity);
        var script = proj.GetComponent<BoomerangProjectile>();
        if (script != null)
            script.Init(dir, speed, range, BuildDamage(damage, ElementType.None, true, 25f), transform);
    }
}
