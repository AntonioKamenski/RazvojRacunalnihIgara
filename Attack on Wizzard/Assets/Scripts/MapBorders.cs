using UnityEngine;

public class MapBorders : MonoBehaviour
{
    public static MapBorders Instance { get; private set; }

    [Header("Map Bounds")]
    public float minX = -80f;
    public float maxX = 80f;
    public float minY = -45f;
    public float maxY = 45f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        DrawRect(minX, maxX, minY, maxY);
    }

    private void DrawRect(float xMin, float xMax, float yMin, float yMax)
    {
        Vector3 topLeft     = new Vector3(xMin, yMax, 0);
        Vector3 topRight    = new Vector3(xMax, yMax, 0);
        Vector3 bottomLeft  = new Vector3(xMin, yMin, 0);
        Vector3 bottomRight = new Vector3(xMax, yMin, 0);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
