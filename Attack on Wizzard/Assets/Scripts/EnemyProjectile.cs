using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 8f;
    [SerializeField] private float rotationOffset = 0f;

    private DamageInfo damageInfo;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    public void Init(Vector2 direction, float speed, DamageInfo info)
    {
        damageInfo = info;
        rb.linearVelocity = direction * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + rotationOffset, Vector3.forward);

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var playerBase = other.GetComponent<PlayerBase>();
        if (playerBase == null) return;

        playerBase.TakeDamage(damageInfo);
        Destroy(gameObject);
    }
}
