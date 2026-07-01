using UnityEngine;
using System.Collections;

public class VoidWraith : BossBase
{
    [Header("Void Wraith")]
    [SerializeField] private GameObject shadowOrbPrefab;
    [SerializeField] private int orbCount = 6;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float orbCooldown = 3f;
    [SerializeField] private float teleportCooldown = 8f;
    [SerializeField] private float cloneCooldown = 20f;
    [SerializeField] private float teleportRange = 6f;
    [SerializeField] private bool isClone = false;

    private float orbTimer;
    private float teleportTimer;
    private float cloneTimer;

    private new void Start()
    {
        bossName = "Thanaros, the Last Reaper";
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (isDead || player == null) return;

        MoveTowardPlayer();

        orbTimer -= Time.deltaTime;
        teleportTimer -= Time.deltaTime;

        if (orbTimer <= 0f)
        {
            orbTimer = orbCooldown;
            FireShadowOrbs();
        }

        if (teleportTimer <= 0f)
        {
            teleportTimer = teleportCooldown;
            TeleportNearPlayer();
        }

        if (isPhaseTwo)
        {
            cloneTimer -= Time.deltaTime;
            if (cloneTimer <= 0f)
            {
                cloneTimer = cloneCooldown;
                StartCoroutine(SpawnShadowClone());
            }
        }
    }

    protected override void OnPhaseTwo()
    {
        orbCooldown = 1.8f;
        teleportCooldown = 5f;
        orbCount += 3;
        chargeSpeed = 3f;
        cloneTimer = 2f;
    }

    private void FireShadowOrbs()
    {
        if (shadowOrbPrefab == null || player == null) return;
        Vector2 dir = ((Vector2)player.position - rb.position).normalized;
        FireCone(shadowOrbPrefab, transform.position, dir, orbCount, 120f, projectileSpeed,
            BuildDamage(25f, ElementType.Shadow, false, 0f));
    }

    private void TeleportNearPlayer()
    {
        if (player == null) return;
        Vector2 offset = Random.insideUnitCircle.normalized * teleportRange;
        transform.position = (Vector2)player.position + offset;
    }

    private IEnumerator SpawnShadowClone()
    {
        if (isClone) yield break;
        Vector2 offset = Random.insideUnitCircle.normalized * 4f;
        var clone = Instantiate(gameObject, (Vector2)transform.position + offset, Quaternion.identity);
        var cloneScript = clone.GetComponent<VoidWraith>();
        if (cloneScript != null)
        {
            cloneScript.isClone = true;
            cloneScript.maxHP = maxHP * 0.25f;
            cloneScript.currentHP = cloneScript.maxHP;
            cloneScript.xpReward = 0;
        }
        Destroy(clone, 12f);
        yield return null;
    }

    private DamageInfo BuildDamage(float amount, ElementType el, bool bleed, float bleedBuildup)
        => new DamageInfo { Amount = amount, Element = el, CausesBleed = bleed, BleedBuildup = bleedBuildup };
}
