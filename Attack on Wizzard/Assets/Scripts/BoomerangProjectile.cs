using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BoomerangProjectile : MonoBehaviour
{
    private DamageInfo damageInfo;
    private Rigidbody2D rb;
    private Transform owner;
    private float speed;
    private float maxDistance;
    private bool returning = false;
    private Vector2 startPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = false;
    }

    public void Init(Vector2 direction, float projectileSpeed, float range, DamageInfo info, Transform ownerTransform)
    {
        damageInfo = info;
        speed = projectileSpeed;
        maxDistance = range;
        owner = ownerTransform;
        startPos = transform.position;

        rb.linearVelocity = direction * speed;
        Destroy(gameObject, 8f);
    }

    private void Update()
    {
        rb.angularVelocity = 600f;

        if (!returning && Vector2.Distance(transform.position, startPos) >= maxDistance)
        {
            returning = true;
        }

        if (returning && owner != null)
        {
            Vector2 dir = ((Vector2)owner.position - rb.position).normalized;
            rb.linearVelocity = dir * speed;

            if (Vector2.Distance(transform.position, owner.position) < 0.5f)
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        var enemy = other.GetComponent<EnemyBase>();
        if (enemy == null || enemy.IsDead) return;

        enemy.TakeDamage(damageInfo);
    }
}
