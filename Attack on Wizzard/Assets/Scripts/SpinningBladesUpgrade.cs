using UnityEngine;
using System.Collections.Generic;

public class SpinningBladesUpgrade : ActiveUpgradeBase
{
    private GameObject bladePrefab;
    private readonly List<Transform> blades = new List<Transform>();
    private float angle = 0f;
    private float rotationSpeed = 180f;
    private float orbitRadius = 1.5f;
    private float bladeHitRadius = 0.7f;
    private float damage = 8f;
    private float perEnemyCooldown = 0.5f;
    private readonly Dictionary<EnemyBase, float> hitTimers = new Dictionary<EnemyBase, float>();

    public void Init(GameObject prefab)
    {
        bladePrefab = prefab;
        SetBladeCount(2);
    }

    protected override void OnLevelUp()
    {
        damage += 4f;
        rotationSpeed += 30f;
        SetBladeCount(2 + level);
    }

    private void SetBladeCount(int count)
    {
        foreach (var b in blades)
            if (b != null) Destroy(b.gameObject);
        blades.Clear();

        if (bladePrefab == null) return;

        for (int i = 0; i < count; i++)
        {
            var blade = Instantiate(bladePrefab, transform).transform;
            blades.Add(blade);
        }
    }

    private void Update()
    {
        angle += rotationSpeed * Time.deltaTime;

        for (int i = 0; i < blades.Count; i++)
        {
            if (blades[i] == null) continue;
            float a = (angle + 360f / blades.Count * i) * Mathf.Deg2Rad;
            blades[i].localPosition = new Vector3(Mathf.Cos(a) * orbitRadius, Mathf.Sin(a) * orbitRadius, 0f);
        }

        // Advance per-enemy cooldown timers
        var keys = new List<EnemyBase>(hitTimers.Keys);
        foreach (var k in keys)
        {
            hitTimers[k] -= Time.deltaTime;
            if (hitTimers[k] <= 0f) hitTimers.Remove(k);
        }

        var dmg = BuildDamage(damage, ElementType.Shadow, true, 20f);
        foreach (var blade in blades)
        {
            if (blade == null) continue;
            foreach (var e in FindObjectsByType<EnemyBase>(FindObjectsSortMode.None))
            {
                if (e.IsDead || hitTimers.ContainsKey(e)) continue;
                if (Vector2.Distance(blade.position, e.transform.position) < bladeHitRadius)
                {
                    e.TakeDamage(dmg);
                    hitTimers[e] = perEnemyCooldown;
                }
            }
        }
    }
}
