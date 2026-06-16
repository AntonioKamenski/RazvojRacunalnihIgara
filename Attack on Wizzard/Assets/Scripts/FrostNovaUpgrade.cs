using UnityEngine;

public class FrostNovaUpgrade : ActiveUpgradeBase
{
    private GameObject iceEffectPrefab;
    [SerializeField] private int spikeCount = 10;

    public void Init(GameObject effectPrefab) => iceEffectPrefab = effectPrefab;

    private float radius = 3f;
    private float damage = 15f;
    private float cooldown = 5f;
    private float timer = 0f;
    private float slowFactor = 0.4f;
    private float slowDuration = 2f;

    protected override void OnLevelUp()
    {
        damage += 8f;
        radius += 0.6f;
        cooldown = Mathf.Max(2f, cooldown - 0.8f);
        spikeCount = Mathf.Min(spikeCount + 2, 20);
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;

        timer = cooldown;
        var dmg = BuildDamage(damage, ElementType.Ice, false, 0f);

        foreach (var e in FindObjectsByType<EnemyBase>(FindObjectsSortMode.None))
        {
            if (!e.IsDead && Vector2.Distance(transform.position, e.transform.position) <= radius)
            {
                e.TakeDamage(dmg);
                e.ApplySlow(slowFactor, slowDuration);
            }
        }

        SpawnIceSpikes();
    }

    private void SpawnIceSpikes()
    {
        if (iceEffectPrefab == null) return;
        for (int i = 0; i < spikeCount; i++)
        {
            float angle = 360f / spikeCount * i;
            float rad   = angle * Mathf.Deg2Rad;
            Vector2 pos = (Vector2)transform.position + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;

            // Rotate sprite to point outward from center
            var go = Instantiate(iceEffectPrefab, pos, Quaternion.Euler(0f, 0f, angle));
            var effect = go.GetComponent<TemporarySpriteEffect>();
            if (effect == null) effect = go.AddComponent<TemporarySpriteEffect>();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.4f, 0.8f, 1f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
