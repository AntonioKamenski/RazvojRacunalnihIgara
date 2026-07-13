using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    // EnemySpawner subscribes to this to count kills per wave
    public static event Action OnEnemyDied;

    [SerializeField] float health = 300f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;

    private float shotCounter;
    private bool isDead = false;

    void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    void Update()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (damageDealer == null) return;
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            isDead = true;
            OnEnemyDied?.Invoke();
            Destroy(gameObject);
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        laser.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, -projectileSpeed);
    }
}
