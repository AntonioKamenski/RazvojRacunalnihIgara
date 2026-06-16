using UnityEngine;

public class PoisonCloudEffect : MonoBehaviour
{
    private float damage;
    private float radius;
    private float duration;
    private DamageInfo damageInfo;
    private float tickInterval = 0.5f;
    private float tickTimer = 0f;

    public void Init(float dmg, float r, float dur, DamageInfo info)
    {
        damage = dmg;
        radius = r;
        duration = dur;
        damageInfo = info;
        Destroy(gameObject, duration);
    }

    private void Update()
    {
        tickTimer -= Time.deltaTime;
        if (tickTimer > 0f) return;

        tickTimer = tickInterval;

        foreach (var e in FindObjectsByType<EnemyBase>(FindObjectsSortMode.None))
        {
            if (!e.IsDead && Vector2.Distance(transform.position, e.transform.position) <= radius)
                e.TakeDamage(damageInfo);
        }
    }
}
