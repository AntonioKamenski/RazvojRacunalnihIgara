using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 6f;
    [SerializeField] private float rotationOffset = 0f;

    [Header("Chill (Mage only)")]
    [SerializeField] private bool appliesChill = false;
    [SerializeField] private float chillSlowFactor = 0.5f;
    [SerializeField] private float chillDuration = 1.5f;

    private DamageInfo damageInfo;
    private Rigidbody2D rb;
    private Transform homingTarget;
    private float homingStrength = 0f;
    private float speed = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    public void Init(Vector2 direction, float projectileSpeed, DamageInfo info,
                     Transform target = null, float homing = 0f)
    {
        damageInfo = info;
        speed = projectileSpeed;
        homingTarget = target;
        homingStrength = homing;

        rb.linearVelocity = direction * speed;
        SetRotation(direction);

        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        if (homingStrength <= 0f || homingTarget == null) return;

        Vector2 dir = ((Vector2)homingTarget.position - rb.position).normalized;
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, dir * speed, homingStrength * Time.fixedDeltaTime);
        SetRotation(rb.linearVelocity.normalized);
    }

    private void SetRotation(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + rotationOffset, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        var enemy = other.GetComponent<EnemyBase>();
        if (enemy == null || enemy.IsDead) return;

        enemy.TakeDamage(damageInfo);

        if (appliesChill)
            enemy.ApplySlow(chillSlowFactor, chillDuration);

        Destroy(gameObject);
    }
}
