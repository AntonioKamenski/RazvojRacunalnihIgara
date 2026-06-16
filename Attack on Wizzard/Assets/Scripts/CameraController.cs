using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Follow Settings")]
    [SerializeField] private float smoothSpeed = 8f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = target.position + offset;

        if (MapBorders.Instance != null)
        {
            // Half the screen size in world units
            float halfH = cam.orthographicSize;
            float halfW = halfH * cam.aspect;

            float clampMinX = MapBorders.Instance.minX + halfW;
            float clampMaxX = MapBorders.Instance.maxX - halfW;
            float clampMinY = MapBorders.Instance.minY + halfH;
            float clampMaxY = MapBorders.Instance.maxY - halfH;

            desired.x = Mathf.Clamp(desired.x, clampMinX, clampMaxX);
            desired.y = Mathf.Clamp(desired.y, clampMinY, clampMaxY);
        }

        Vector3 newPos = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);

        // Second clamp prevents lerp overshoot at edges
        if (MapBorders.Instance != null)
        {
            float halfH = cam.orthographicSize;
            float halfW = halfH * cam.aspect;

            newPos.x = Mathf.Clamp(newPos.x, MapBorders.Instance.minX + halfW, MapBorders.Instance.maxX - halfW);
            newPos.y = Mathf.Clamp(newPos.y, MapBorders.Instance.minY + halfH, MapBorders.Instance.maxY - halfH);
        }

        newPos.z = offset.z;
        transform.position = newPos;
    }

    public void SetTarget(Transform newTarget) => target = newTarget;
}
