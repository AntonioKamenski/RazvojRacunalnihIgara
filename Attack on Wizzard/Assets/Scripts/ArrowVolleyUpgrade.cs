using UnityEngine;

public class ArrowVolleyUpgrade : ActiveUpgradeBase
{
    private GameObject arrowPrefab;
    private float cooldown = 2f;
    private float timer = 0f;
    private float arrowSpeed = 10f;
    private float damage = 12f;

    public void Init(GameObject prefab) => arrowPrefab = prefab;

    protected override void OnLevelUp()
    {
        damage += 4f;
        cooldown = Mathf.Max(0.8f, cooldown - 0.3f);
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
        if (arrowPrefab == null) return;

        int count = 3 + (level - 1) * 2;
        float spread = 30f + (level - 1) * 10f;
        Vector2 baseDir = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
        float baseAngle = Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg;

        for (int i = 0; i < count; i++)
        {
            float angle = baseAngle - spread * 0.5f + spread * i / (count - 1);
            float rad = angle * Mathf.Deg2Rad;
            var dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            var proj = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            var script = proj.GetComponent<PlayerProjectile>();
            if (script != null)
                script.Init(dir, arrowSpeed, BuildDamage(damage, ElementType.None, true, 15f));
        }
    }
}
