using UnityEngine;
using UnityEngine.InputSystem;


public class ball : MonoBehaviour
{
    [SerializeField] Paddle paddle;
    [SerializeField] AudioClip[] ballSounds;
    Vector2 paddleToBallVector;
    bool hasStarted = false;

    AudioSource myAudioSource;

    void Start()
    {
        paddleToBallVector = transform.position - paddle.transform.position;
        myAudioSource = GetComponent<AudioSource>();

    }
    void Update()
    {
        if (!hasStarted)
        {
            lockBallToPaddle();
            shootOnMouseClick();
        }
    }

    void lockBallToPaddle()
    {
        Vector2 paddlePos = new Vector2(paddle.transform.position.x, paddle.transform.position.y);
        transform.position = paddlePos + paddleToBallVector;
    }

    private void shootOnMouseClick()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            hasStarted = true;
            Debug.Log("Space Key Pressed");
            GetComponent<Rigidbody2D>().linearVelocity = new Vector2(2f, 15f);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 velocityTweak = new Vector2(UnityEngine.Random.Range(0f, 0.2f), UnityEngine.Random.Range(0f, 0.2f));
        GetComponent<Rigidbody2D>().linearVelocity += velocityTweak;
        AudioClip clip = ballSounds[UnityEngine.Random.Range(0, ballSounds.Length)];
        myAudioSource.PlayOneShot(clip);
    }
}
