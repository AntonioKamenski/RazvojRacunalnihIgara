using UnityEngine;
using System.Collections;

public class GlacialColossus : BossBase
{
    [Header("Glacial Colossus")]
    [SerializeField] private GameObject iceSpikePrefab;
    [SerializeField] private int spikeCount = 8;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float spikeCooldown = 4f;
    [SerializeField] private float beamCooldown = 10f;
    [SerializeField] private float beamSlowFactor = 0.35f;
    [SerializeField] private float beamDuration = 3f;
    [SerializeField] private float beamTickInterval = 0.5f;
    [SerializeField] private float beamRange = 10f;

    private float spikeTimer;
    private float beamTimer;
    private bool isFiring;

    private new void Start()
    {
        bossName = "The Glacial Colossus";
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (isDead || player == null) return;

        if (!isFiring) MoveTowardPlayer();

        spikeTimer -= Time.deltaTime;
        beamTimer -= Time.deltaTime;

        if (spikeTimer <= 0f)
        {
            spikeTimer = spikeCooldown;
            IceSpikes();
        }

        if (beamTimer <= 0f)
        {
            beamTimer = beamCooldown;
            StartCoroutine(FreezeBeam());
        }
    }

    protected override void OnPhaseTwo()
    {
        spikeCooldown = 2.5f;
        beamCooldown = 7f;
        spikeCount += 4;
        chargeSpeed = 4f;
    }

    private void IceSpikes()
    {
        if (iceSpikePrefab == null || player == null) return;
        Vector2 dir = ((Vector2)player.position - rb.position).normalized;
        int count = isPhaseTwo ? spikeCount : spikeCount;
        FireCone(iceSpikePrefab, transform.position, dir, count, 90f, projectileSpeed,
            BuildDamage(22f, ElementType.Ice, false, 0f));
    }

    private IEnumerator FreezeBeam()
    {
        isFiring = true;
        StopMoving();

        float elapsed = 0f;
        float tick = 0f;

        PlayerBase pb = player != null ? player.GetComponent<PlayerBase>() : null;
        float originalSpeed = pb != null ? pb.MoveSpeed : 1f;
        if (pb != null) pb.SetMoveSpeed(originalSpeed * beamSlowFactor);

        while (elapsed < beamDuration)
        {
            if (player == null) break;
            float dist = Vector2.Distance(transform.position, player.position);
            if (dist <= beamRange)
            {
                tick += Time.deltaTime;
                if (tick >= beamTickInterval)
                {
                    tick = 0f;
                    pb?.TakeDamage(BuildDamage(18f, ElementType.Ice, false, 0f));
                }
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (pb != null) pb.SetMoveSpeed(originalSpeed);
        isFiring = false;
    }

    private DamageInfo BuildDamage(float amount, ElementType el, bool bleed, float bleedBuildup)
        => new DamageInfo { Amount = amount, Element = el, CausesBleed = bleed, BleedBuildup = bleedBuildup };
}
