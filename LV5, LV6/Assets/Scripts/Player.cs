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
    Coroutine firing;

    void Start()
    {
        SetupBoundaries();

        transform.position = new Vector2(0, yMin + 0.5f);
    }

    void Update()
    {
        move(); 
        fire();  
    }
    private void move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPosition = transform.position.x + deltaX;
        var newYPosition = transform.position.y + deltaY;

        newXPosition = Mathf.Clamp(newXPosition, xMin, xMax);
        newYPosition = Mathf.Clamp(newYPosition, yMin, yMax);

        transform.position = new Vector2(newXPosition, newYPosition);
    }

    private void SetupBoundaries()
    {
        var camera = Camera.main;
        xMin = camera.ViewportToWorldPoint(new Vector2(0 + padding, 0)).x;
        xMax = camera.ViewportToWorldPoint(new Vector2(1 - padding, 0)).x;
        yMin = camera.ViewportToWorldPoint(new Vector2(0, 0.03f)).y;
        yMax = camera.ViewportToWorldPoint(new Vector2(0, 0.7f)).y;
    }
    private void fire()
    {
        if (Input.GetButtonDown("Jump"))
        {
            firing = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Jump"))
        {
            StopCoroutine(firing);
        }
    }
    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, projectileSpeed);
            yield return new WaitForSeconds(firingRate);
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
}
