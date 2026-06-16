using UnityEngine;
using System.Collections;

public class BossBase : EnemyBase
{
    [Header("Boss Identity")]
    [SerializeField] protected string bossName = "Boss";

    [Header("Phase 2")]
    [SerializeField] protected float phase2Threshold = 0.5f;

    [Header("Movement")]
    [SerializeField] protected float chargeSpeed = 2f;

    protected bool isPhaseTwo = false;
    protected Transform player;

    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void Start()
    {
        var playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        BossHUDController.Instance?.Show(bossName, this);
        AudioManager.Instance?.PlayBossRoar();
    }

    protected virtual void Update()
    {
        if (isDead || player == null) return;

        if (!isPhaseTwo && currentHP / maxHP <= phase2Threshold)
        {
            isPhaseTwo = true;
            OnPhaseTwo();
        }
    }

    protected virtual void OnPhaseTwo() { }

    protected void MoveTowardPlayer()
    {
        if (player == null) return;
        Vector2 dir = ((Vector2)player.position - rb.position).normalized;
        rb.linearVelocity = dir * chargeSpeed;

        var sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.flipX = dir.x < 0;
    }

    protected void StopMoving() => rb.linearVelocity = Vector2.zero;

    protected void FireProjectile(GameObject prefab, Vector2 origin, Vector2 direction,
                                   float speed, DamageInfo dmg)
    {
        if (prefab == null) return;
        var go = Instantiate(prefab, origin, Quaternion.identity);
        var proj = go.GetComponent<EnemyProjectile>();
        if (proj != null) proj.Init(direction.normalized, speed, dmg);
        AudioManager.Instance?.PlayEnemyRanged();
    }

    protected void FireCircle(GameObject prefab, Vector2 origin, int count,
                               float speed, DamageInfo dmg)
    {
        for (int i = 0; i < count; i++)
        {
            float rad = 360f / count * i * Mathf.Deg2Rad;
            FireProjectile(prefab, origin, new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)), speed, dmg);
        }
    }

    protected void FireCone(GameObject prefab, Vector2 origin, Vector2 toward,
                             int count, float spreadDeg, float speed, DamageInfo dmg)
    {
        float baseAngle = Mathf.Atan2(toward.y, toward.x) * Mathf.Rad2Deg;
        for (int i = 0; i < count; i++)
        {
            float angle = baseAngle - spreadDeg * 0.5f + spreadDeg * i / Mathf.Max(1, count - 1);
            float rad = angle * Mathf.Deg2Rad;
            FireProjectile(prefab, origin, new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)), speed, dmg);
        }
    }

    protected void FireLine(GameObject prefab, Vector2 origin, Vector2 direction,
                             int count, float spacing, float speed, DamageInfo dmg)
    {
        Vector2 perp = new Vector2(-direction.y, direction.x);
        float totalWidth = spacing * (count - 1);
        for (int i = 0; i < count; i++)
        {
            Vector2 offset = perp * (-totalWidth * 0.5f + spacing * i);
            FireProjectile(prefab, origin + offset, direction, speed, dmg);
        }
    }

    protected IEnumerator TelegraphThenFire(Vector2 targetPos, float delay,
                                             System.Action onFire)
    {
        // Placeholder for telegraph visual — can add a marker here later
        yield return new WaitForSeconds(delay);
        onFire?.Invoke();
    }

    protected override void Die()
    {
        BossHUDController.Instance?.Hide();
        AudioManager.Instance?.PlayBossDeath();
        GameEndManager.Instance?.ShowVictory(bossName);
        base.Die();
    }
}
