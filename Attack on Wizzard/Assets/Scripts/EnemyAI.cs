using UnityEngine;

[RequireComponent(typeof(EnemyBase))]
public class EnemyAI : MonoBehaviour
{
    [Header("Melee Settings")]
    [SerializeField] private float attackRange = 0.8f;

    private EnemyBase enemy;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private EnemyAnimator enemyAnimator;
    private PlayerBase player;
    private float attackTimer = 0f;

    private void Awake()
    {
        enemy = GetComponent<EnemyBase>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyAnimator = GetComponent<EnemyAnimator>();
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

        Vector2 direction = ((Vector2)player.transform.position - rb.position).normalized;
        rb.linearVelocity = direction * enemy.MoveSpeed;

        if (spriteRenderer != null)
        {
            bool shouldFlip = direction.x < 0;
            if (enemyAnimator != null && enemyAnimator.FlipFacingOnX)
                shouldFlip = !shouldFlip;

            spriteRenderer.flipX = shouldFlip;
        }

        enemy.ClampToBorders();

        float distance = Vector2.Distance(rb.position, player.transform.position);
        if (distance <= attackRange && attackTimer <= 0f)
        {
            player.TakeDamage(DamageInfo.Pure(enemy.ContactDamage));
            attackTimer = enemy.AttackCooldown;
            enemyAnimator?.TriggerAttack();
            AudioManager.Instance?.PlayEnemyMelee();
        }
    }
}
