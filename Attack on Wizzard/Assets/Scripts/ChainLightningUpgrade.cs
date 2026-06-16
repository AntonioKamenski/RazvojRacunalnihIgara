using UnityEngine;
using System.Collections.Generic;

public class ChainLightningUpgrade : ActiveUpgradeBase
{
    private GameObject lightningEffectPrefab;
    private float cooldown = 3f;
    private float timer = 0f;
    private float damage = 20f;
    private float chainRange = 5f;
    private int chains = 3;

    public void Init(GameObject effectPrefab) => lightningEffectPrefab = effectPrefab;

    protected override void OnLevelUp()
    {
        damage += 8f;
        chains++;
        cooldown = Mathf.Max(1f, cooldown - 0.4f);
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;

        var first = FindNearestEnemy(transform.position);
        if (first == null) return;

        Fire(first);
        timer = cooldown;
    }

    private void Fire(EnemyBase startEnemy)
    {
        var dmg = BuildDamage(damage, ElementType.Lightning, false, 0f);
        var hit = startEnemy;
        var alreadyHit = new HashSet<EnemyBase> { hit };

        for (int i = 0; i < chains; i++)
        {
            if (hit == null || hit.IsDead) break;
            hit.TakeDamage(dmg);
            SpawnEffect(hit.transform.position);

            var next = FindNextChainTarget(hit, alreadyHit);
            if (next == null) break;
            alreadyHit.Add(next);
            hit = next;
        }
    }

    private void SpawnEffect(Vector2 pos)
    {
        if (lightningEffectPrefab == null) return;
        var go = Instantiate(lightningEffectPrefab, pos, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
        var effect = go.GetComponent<TemporarySpriteEffect>();
        if (effect == null) effect = go.AddComponent<TemporarySpriteEffect>();
    }

    private EnemyBase FindNextChainTarget(EnemyBase from, HashSet<EnemyBase> exclude)
    {
        EnemyBase nearest = null;
        float nearestDist = chainRange;

        foreach (var e in FindObjectsByType<EnemyBase>(FindObjectsSortMode.None))
        {
            if (e.IsDead || exclude.Contains(e)) continue;
            float d = Vector2.Distance(from.transform.position, e.transform.position);
            if (d < nearestDist) { nearestDist = d; nearest = e; }
        }
        return nearest;
    }
}
