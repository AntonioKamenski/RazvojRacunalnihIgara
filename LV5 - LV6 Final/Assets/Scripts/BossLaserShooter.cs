using UnityEngine;

// Attach one of these to each of the four child shooter points on the Boss prefab.
// Set a different spreadAngle on each so lasers fan outward.
public class BossLaserShooter : MonoBehaviour
{
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed    = 8f;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 0.9f;

    // Degrees from straight-down. Negative = left, positive = right.
    // Suggested values for the four shooters: -30, -10, 10, 30
    [SerializeField] float spreadAngle = 0f;

    private float shotTimer;

    void Start()
    {
        shotTimer = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    void Update()
    {
        shotTimer -= Time.deltaTime;
        if (shotTimer <= 0f)
        {
            Fire();
            shotTimer = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        float rad = spreadAngle * Mathf.Deg2Rad;
        // Shoot downward with a horizontal spread determined by spreadAngle
        Vector2 velocity = new Vector2(Mathf.Sin(rad), -Mathf.Cos(rad)) * projectileSpeed;

        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        laser.GetComponent<Rigidbody2D>().linearVelocity = velocity;
    }
}
