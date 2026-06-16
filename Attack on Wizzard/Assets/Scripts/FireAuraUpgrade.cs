using UnityEngine;

public class FireAuraUpgrade : ActiveUpgradeBase
{
    private GameObject fireEffectPrefab;
    [SerializeField] private int effectsPerTick = 6;

    public void Init(GameObject effectPrefab) => fireEffectPrefab = effectPrefab;

    private float radius = 2f;
    private float damage = 5f;
    private float tickInterval = 0.5f;
    private float timer = 0f;

    protected override void OnLevelUp()
    {
        damage += 3f;
        radius += 0.4f;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;

        timer = tickInterval;
        var dmg = BuildDamage(damage, ElementType.Fire, false, 0f);

        bool hitAny = false;
        foreach (var e in FindObjectsByType<EnemyBase>(FindObjectsSortMode.None))
        {
            if (!e.IsDead && Vector2.Distance(transform.position, e.transform.position) <= radius)
            {
                e.TakeDamage(dmg);
                hitAny = true;
            }
        }

        if (hitAny || fireEffectPrefab != null)
            SpawnFireEffects();
    }

    private void SpawnFireEffects()
    {
        if (fireEffectPrefab == null) return;
        for (int i = 0; i < effectsPerTick; i++)
        {
            Vector2 offset = Random.insideUnitCircle * radius;
            var go = Instantiate(fireEffectPrefab,
                                 (Vector2)transform.position + offset,
                                 Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
            var effect = go.GetComponent<TemporarySpriteEffect>();
            if (effect == null) effect = go.AddComponent<TemporarySpriteEffect>();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.3f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
