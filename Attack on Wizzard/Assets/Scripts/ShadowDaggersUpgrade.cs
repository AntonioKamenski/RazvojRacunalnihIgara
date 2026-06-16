using UnityEngine;

public class ShadowDaggersUpgrade : ActiveUpgradeBase
{
    private GameObject daggerPrefab;
    private float cooldown = 0.8f;
    private float timer = 0f;
    private float speed = 9f;
    private float damage = 10f;
    private int daggerCount = 1;

    public void Init(GameObject prefab) => daggerPrefab = prefab;

    protected override void OnLevelUp()
    {
        damage += 5f;
        daggerCount++;
        cooldown = Mathf.Max(0.3f, cooldown - 0.15f);
    }

    private void Update()
    {
        if (!playerBase.IsMoving) return;

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        timer = cooldown;
        Fire();
    }

    private void Fire()
    {
        if (daggerPrefab == null) return;

        var target = FindNearestEnemy(transform.position);
        Vector2 baseDir = target != null
            ? -((Vector2)target.transform.position - (Vector2)transform.position).normalized
            : -Vector2.up;

        float spread = daggerCount > 1 ? 25f : 0f;

        for (int i = 0; i < daggerCount; i++)
        {
            float offset = daggerCount > 1
                ? -spread * 0.5f + spread * i / (daggerCount - 1)
                : 0f;

            float rad = (Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg + offset) * Mathf.Deg2Rad;
            var dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            var proj = Instantiate(daggerPrefab, transform.position, Quaternion.identity);
            var script = proj.GetComponent<PlayerProjectile>();
            if (script != null)
                script.Init(dir, speed, BuildDamage(damage, ElementType.Shadow, true, 28f));
        }
    }
}
