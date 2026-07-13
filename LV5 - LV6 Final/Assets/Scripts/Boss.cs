using UnityEngine;
using System;

public class Boss : MonoBehaviour
{
    // Passes current health fraction (0-1) to subscribers
    public static event Action<float> OnHealthChanged;

    [SerializeField] float health = 3000f;
    [SerializeField] float moveSpeed = 2.5f;
    [SerializeField] float leftBound  = -7f;
    [SerializeField] float rightBound =  7f;

    private float maxHealth;
    private float direction = 1f;

    void Start()
    {
        maxHealth = health;
        OnHealthChanged?.Invoke(1f);
    }

    void Update()
    {
        transform.position += new Vector3(direction * moveSpeed * Time.deltaTime, 0f, 0f);

        if (transform.position.x >= rightBound || transform.position.x <= leftBound)
            direction *= -1f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (damageDealer == null) return;

        health -= damageDealer.GetDamage();
        damageDealer.Hit();

        OnHealthChanged?.Invoke(Mathf.Clamp01(health / maxHealth));

        if (health <= 0)
        {
            GameManager.Instance?.BossDefeated();
            Destroy(gameObject);
        }
    }
}
