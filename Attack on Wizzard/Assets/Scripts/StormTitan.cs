using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StormTitan : BossBase
{
    [Header("Storm Titan")]
    [SerializeField] private GameObject lightningBoltPrefab;
    [SerializeField] private float strikeCooldown = 2.5f;
    [SerializeField] private float chainCooldown = 7f;
    [SerializeField] private int chainCount = 3;
    [SerializeField] private float chainRange = 8f;
    [SerializeField] private float groundZoneCooldown = 12f;

    private float strikeTimer;
    private float chainTimer;
    private float groundTimer;

    private new void Start()
    {
        bossName = "Vaelith, Keeper of the Arcane Throne";
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (isDead || player == null) return;

        MoveTowardPlayer();

        strikeTimer -= Time.deltaTime;
        chainTimer -= Time.deltaTime;
        groundTimer -= Time.deltaTime;

        if (strikeTimer <= 0f)
        {
            strikeTimer = strikeCooldown;
            StartCoroutine(LightningStrike());
        }

        if (chainTimer <= 0f)
        {
            chainTimer = chainCooldown;
            ChainBolt();
        }

        if (groundTimer <= 0f)
        {
            groundTimer = groundZoneCooldown;
            StartCoroutine(ElectrifiedGround());
        }
    }

    protected override void OnPhaseTwo()
    {
        strikeCooldown = 1.5f;
        chainCount += 2;
        groundZoneCooldown = 7f;
        chargeSpeed = 3f;
    }

    private IEnumerator LightningStrike()
    {
        if (player == null) yield break;
        Vector2 target = player.position;
        yield return StartCoroutine(TelegraphThenFire(target, 0.5f, () =>
        {
            if (lightningBoltPrefab != null)
            {
                var go = Instantiate(lightningBoltPrefab, target, Quaternion.identity);
                var dmg = BuildDamage(40f, ElementType.Lightning, true, 18f);
                var col = go.GetComponent<CircleCollider2D>();
                if (col == null) col = go.AddComponent<CircleCollider2D>();
                col.isTrigger = true;
                col.radius = 1.2f;
                go.AddComponent<InstantAoEDamage>().Init(dmg, "Player");
                Destroy(go, 0.3f);
            }
        }));
    }

    private void ChainBolt()
    {
        var enemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);
        var hit = new HashSet<EnemyBase>();

        if (player == null) return;
        var pb = player.GetComponent<PlayerBase>();
        if (pb == null) return;

        pb.TakeDamage(BuildDamage(20f, ElementType.Lightning, false, 0f));

        int chains = 0;
        EnemyBase last = null;

        foreach (var e in enemies)
        {
            if (chains >= chainCount) break;
            if (hit.Contains(e) || e == this) continue;
            float dist = last != null
                ? Vector2.Distance(last.transform.position, e.transform.position)
                : Vector2.Distance(player.position, e.transform.position);
            if (dist <= chainRange)
            {
                e.TakeDamage(BuildDamage(15f, ElementType.Lightning, true, 10f));
                hit.Add(e);
                last = e;
                chains++;
            }
        }
    }

    private IEnumerator ElectrifiedGround()
    {
        if (player == null) yield break;
        Vector2 center = player.position;
        float duration = 5f;
        float tick = 0f;
        float tickInterval = 0.5f;
        float radius = 3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            tick += Time.deltaTime;
            elapsed += Time.deltaTime;
            if (tick >= tickInterval && player != null)
            {
                tick = 0f;
                if (Vector2.Distance(center, player.position) <= radius)
                    player.GetComponent<PlayerBase>()?.TakeDamage(BuildDamage(12f, ElementType.Lightning, false, 0f));
            }
            yield return null;
        }
    }

    private DamageInfo BuildDamage(float amount, ElementType el, bool bleed, float bleedBuildup)
        => new DamageInfo { Amount = amount, Element = el, CausesBleed = bleed, BleedBuildup = bleedBuildup };
}
