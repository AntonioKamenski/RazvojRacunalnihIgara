using UnityEngine;

[RequireComponent(typeof(EnemyBase))]
public class RangedEnemyAI : MonoBehaviour
{
    [Header("Ranged Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float preferredDistance = 7f;
    [SerializeField] private float retreatThreshold = 0.7f;
    [SerializeField] private float projectileSpeed = 6f;
    [SerializeField] private Transform firePoint;

    private EnemyBase enemy;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private PlayerBase player;
    private float attackTimer = 0f;

    private void Awake()
    {
        enemy = GetComponent<EnemyBase>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        var playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            player = playerObj.GetComponent<PlayerBase>();
    }

    private void FixedUpdate()
    {
        if (enemy.IsDead || player == null || player.IsDead) return;

        attackTimer -= Time.fixedDeltaTime;

        float distance = Vector2.Distance(rb.position, player.transform.position);
        Vector2 toPlayer = ((Vector2)player.transform.position - rb.position).normalized;

        if (distance > preferredDistance)
            rb.linearVelocity = toPlayer * enemy.MoveSpeed;
        else if (distance < preferredDistance * retreatThreshold)
            rb.linearVelocity = -toPlayer * enemy.MoveSpeed;
        else
            rb.linearVelocity = Vector2.zero;

        if (spriteRenderer != null)
            spriteRenderer.flipX = toPlayer.x < 0;

        enemy.ClampToBorders();

        if (attackTimer <= 0f && distance <= preferredDistance + 2f)
        {
            Fire(toPlayer);
            attackTimer = enemy.AttackCooldown;
        }
    }

    private void Fire(Vector2 direction)
    {
        if (projectilePrefab == null) return;

        Vector3 origin = firePoint != null ? firePoint.position : transform.position;
        var proj = Instantiate(projectilePrefab, origin, Quaternion.identity);

        var projScript = proj.GetComponent<EnemyProjectile>();
        if (projScript != null)
            projScript.Init(direction, projectileSpeed, new DamageInfo(enemy.ContactDamage));

        AudioManager.Instance?.PlayEnemyRanged();
    }
}
