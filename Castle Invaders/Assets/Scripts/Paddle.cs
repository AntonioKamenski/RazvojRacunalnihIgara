using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour
{
    [SerializeField] private float min = -7f;
    [SerializeField] private float max = 7f;
    [SerializeField] private float position_multiplier = 0.3f;
    void Update()
    {
        MoveMouse();
    }
    private void MoveMouse()
    {
        Debug.Log(Pointer.current.position.ReadValue().x / Screen.width * 16);
        float mousePos = Pointer.current.position.ReadValue().x / Screen.width * 16f;
        mousePos = CorrectDeflection(mousePos, position_multiplier);
        Vector2 paddlePos = new Vector2(mousePos, transform.position.y);
        paddlePos.x = Mathf.Clamp(mousePos, min, max);

        transform.position = paddlePos;
    }

        public static float CorrectDeflection(float raw, float k = 0.3f)
    {
        float d = Mathf.Abs(raw - 8f);
        float mult = 1f + k * (d / 8f) * (d / 8f);
        return 8f + (raw - 8f) / mult;
    }
}
