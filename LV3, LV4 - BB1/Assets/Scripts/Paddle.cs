using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour
{
    [SerializeField] private float min = 1.1f;
    [SerializeField] private float max = 14.9f;
    void Update()
    {
        MoveMouse();
    }

    private void MoveMouse()
    {
        Debug.Log(Mouse.current.position.ReadValue().x / Screen.width * 16);
        float mousePos = Pointer.current.position.ReadValue().x / Screen.width * 16;
        Vector2 paddlePos = new Vector2(mousePos, transform.position.y);
        paddlePos.x = Mathf.Clamp(mousePos, min, max);

        transform.position = paddlePos;
    }
}
