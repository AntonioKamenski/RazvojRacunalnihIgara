using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 300f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    void Start()
    {
        
    }

    void Update()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f){
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision){
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0){
            Destroy(gameObject);
        }
    }
    private void Fire()
    {
        GameObject laser = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, -projectileSpeed);
    }
}
