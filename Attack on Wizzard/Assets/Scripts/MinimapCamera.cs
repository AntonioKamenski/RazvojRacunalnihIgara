using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MinimapCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Settings")]
    [SerializeField] private float orthographicSize = 40f;
    [SerializeField] private bool followPlayer = false;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = orthographicSize;
    }

    private void LateUpdate()
    {
        if (!followPlayer || target == null) return;
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }

    public void SetTarget(Transform newTarget) => target = newTarget;
}
