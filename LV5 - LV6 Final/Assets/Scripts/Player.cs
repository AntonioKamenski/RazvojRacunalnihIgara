using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    float xMin, xMax, yMin, yMax;
    float padding = 0.05f;

    [SerializeField] float moveSpeed = 10f;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float firingRate = 0.1f;
    [SerializeField] float health = 500f;

    // Upgraded by powerups
    private int laserCount = 1;
    private int bonusDamage = 0;

    private Coroutine firing;

    void Start()
    {
        SetupBoundaries();
        transform.position = new Vector2(0, yMin + 0.5f);
    }

    void Update()
    {
        if (GameManager.Instance == null) return;
        var state = GameManager.Instance.CurrentState;

        if (state != GameManager.GameState.Playing && state != GameManager.GameState.BossFight)
        {
            if (firing != null) { StopCoroutine(firing); firing = null; }
            return;
        }

        Move();
        HandleFire();
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical")   * Time.deltaTime * moveSpeed;

        var newX = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newY = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newX, newY);
    }

    private void SetupBoundaries()
    {
        var cam = Camera.main;
        xMin = cam.ViewportToWorldPoint(new Vector2(padding,     0)).x;
        xMax = cam.ViewportToWorldPoint(new Vector2(1 - padding, 0)).x;
        yMin = cam.ViewportToWorldPoint(new Vector2(0, 0.03f)).y;
        yMax = cam.ViewportToWorldPoint(new Vector2(0, 0.7f)).y;
    }

    private void HandleFire()
    {
        if (Input.GetButtonDown("Jump"))
            firing = StartCoroutine(FireContinuously());
        if (Input.GetButtonUp("Jump") && firing != null)
            StopCoroutine(firing);
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            SpawnLasers();
            yield return new WaitForSeconds(firingRate);
        }
    }

    private void SpawnLasers()
    {
        if (laserCount >= 3)
        {
            SpawnLaserAt(transform.position + Vector3.left  * 0.5f);
            SpawnLaserAt(transform.position);
            SpawnLaserAt(transform.position + Vector3.right * 0.5f);
        }
        else if (laserCount == 2)
        {
            SpawnLaserAt(transform.position + Vector3.left  * 0.3f);
            SpawnLaserAt(transform.position + Vector3.right * 0.3f);
        }
        else
        {
            SpawnLaserAt(transform.position);
        }
    }

    private void SpawnLaserAt(Vector3 position)
    {
        GameObject laser = Instantiate(laserPrefab, position, Quaternion.identity);
        laser.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, projectileSpeed);
        var dd = laser.GetComponent<DamageDealer>();
        if (dd != null) dd.damage += bonusDamage;
    }

    public void ApplyPowerup(PowerupManager.PowerupType type)
    {
        switch (type)
        {
            case PowerupManager.PowerupType.HPUp:
                health += 100f;
                break;
            case PowerupManager.PowerupType.DamageUp:
                bonusDamage += 50;
                break;
            case PowerupManager.PowerupType.FireRateUp:
                firingRate = Mathf.Max(0.05f, firingRate * 0.7f);
                break;
            case PowerupManager.PowerupType.ProjectileSpeedUp:
                projectileSpeed += 5f;
                break;
            case PowerupManager.PowerupType.DualLaser:
                laserCount = Mathf.Max(laserCount, 2);
                break;
            case PowerupManager.PowerupType.TripleLaser:
                laserCount = 3;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (damageDealer == null) return;
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            GameManager.Instance?.PlayerDied();
            Destroy(gameObject);
        }
    }
}
