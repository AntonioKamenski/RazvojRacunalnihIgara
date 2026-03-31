using UnityEngine;
using UnityEngine.InputSystem;


public class ball : MonoBehaviour
{
    [SerializeField] Paddle paddle;
    Vector2 paddleToBallVector;
    bool hasStarted = false;

    void Start()
    {
        paddleToBallVector = transform.position - paddle.transform.position;

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
        //if (Keyboard.current.spaceKey.wasPressedThisFrame)
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            hasStarted = true;
            Debug.Log("Space Key Pressed");
            GetComponent<Rigidbody2D>().linearVelocity = new Vector2(2f, 15f);
        }
    }
}
