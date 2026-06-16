using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Map Borders (leave at 0 to use MapBorders component)")]
    [SerializeField] private bool overrideBorders = false;
    [SerializeField] private float minX = -16f;
    [SerializeField] private float maxX = 16f;
    [SerializeField] private float minY = -9f;
    [SerializeField] private float maxY = 9f;
    [SerializeField] private float borderPaddingX = 1f;
    [SerializeField] private float borderPaddingY = 1f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        float x = (kb.dKey.isPressed || kb.rightArrowKey.isPressed ? 1f : 0f)
                - (kb.aKey.isPressed || kb.leftArrowKey.isPressed  ? 1f : 0f);
        float y = (kb.wKey.isPressed || kb.upArrowKey.isPressed    ? 1f : 0f)
                - (kb.sKey.isPressed || kb.downArrowKey.isPressed   ? 1f : 0f);

        moveInput = new Vector2(x, y).normalized;

        if (spriteRenderer != null && x != 0)
            spriteRenderer.flipX = x < 0;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
        ClampToBorders();
    }

    private void ClampToBorders()
    {
        float bMinX, bMaxX, bMinY, bMaxY;

        if (!overrideBorders && MapBorders.Instance != null)
        {
            bMinX = MapBorders.Instance.minX;
            bMaxX = MapBorders.Instance.maxX;
            bMinY = MapBorders.Instance.minY;
            bMaxY = MapBorders.Instance.maxY;
        }
        else
        {
            bMinX = minX;
            bMaxX = maxX;
            bMinY = minY;
            bMaxY = maxY;
        }

        Vector2 pos = rb.position;
        pos.x = Mathf.Clamp(pos.x, bMinX + borderPaddingX, bMaxX - borderPaddingX);
        pos.y = Mathf.Clamp(pos.y, bMinY + borderPaddingY, bMaxY - borderPaddingY);
        rb.position = pos;
    }
}
