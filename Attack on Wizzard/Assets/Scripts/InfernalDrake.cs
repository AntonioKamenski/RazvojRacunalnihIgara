using UnityEngine;
using System.Collections;

public class InfernalDrake : BossBase
{
    [Header("Infernal Drake")]
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private int coneCount = 7;
    [SerializeField] private float coneSpread = 60f;
    [SerializeField] private float projectileSpeed = 6f;
    [SerializeField] private float breathCooldown = 3f;
    [SerializeField] private float novaCooldown = 8f;
    [SerializeField] private float summonCooldown = 15f;

    private float breathTimer;
    private float novaTimer;
    private float summonTimer;

    private new void Start()
    {
        bossName = "The Infernal Drake";
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (isDead || player == null) return;

        MoveTowardPlayer();

        breathTimer -= Time.deltaTime;
        novaTimer -= Time.deltaTime;

        if (breathTimer <= 0f)
        {
            breathTimer = breathCooldown;
            StartCoroutine(FireBreath());
        }

        if (novaTimer <= 0f)
        {
            novaTimer = novaCooldown;
            FireCircle(fireballPrefab, transform.position, 12, projectileSpeed,
                BuildDamage(35f, ElementType.Fire, true, 20f));
        }

        if (isPhaseTwo)
        {
            summonTimer -= Time.deltaTime;
            if (summonTimer <= 0f)
            {
                summonTimer = summonCooldown;
                SummonMinions();
            }
        }
    }

    protected override void OnPhaseTwo()
    {
        chargeSpeed = 6f;
        breathCooldown = 2f;
        novaCooldown = 5f;
        summonTimer = 2f;
    }

    private IEnumerator FireBreath()
    {
        if (player == null) yield break;
        Vector2 dir = ((Vector2)player.position - rb.position).normalized;
        yield return StartCoroutine(TelegraphThenFire(player.position, 0.6f, () =>
        {
            FireCone(fireballPrefab, transform.position, dir, coneCount, coneSpread,
                projectileSpeed, BuildDamage(28f, ElementType.Fire, true, 15f));
        }));
    }

    private void SummonMinions()
    {
        if (minionPrefab == null) return;
        for (int i = 0; i < 3; i++)
        {
            Vector2 offset = Random.insideUnitCircle * 2.5f;
            Instantiate(minionPrefab, (Vector2)transform.position + offset, Quaternion.identity);
        }
    }

    private DamageInfo BuildDamage(float amount, ElementType el, bool bleed, float bleedBuildup)
        => new DamageInfo { Amount = amount, Element = el, CausesBleed = bleed, BleedBuildup = bleedBuildup };
}
