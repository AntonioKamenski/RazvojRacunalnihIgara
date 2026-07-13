using UnityEngine;
using System.Collections.Generic;

public class EnemyPath : MonoBehaviour
{
    [SerializeField] WaveConfig waveConfig;
    [SerializeField] List<Transform> pathPoints;
    //[SerializeField] float moveSpeed = 5f;
    //[SerializeField] List<Transform> pathPoints = new List<Transform>();
    private int waypointIndex = 0;


    void Start()
    {
        pathPoints = waveConfig.GetWaypoints();
        if (pathPoints == null || pathPoints.Count == 0)
        {
            Debug.LogError("EnemyPath requires at least one waypoint assigned in the Inspector.");
            enabled = false;
            return;
        }

        waypointIndex = GetNextValidWaypointIndex(0);
        if (waypointIndex == -1)
        {
            Debug.LogError("EnemyPath has no valid waypoint transforms assigned.");
            enabled = false;
            return;
        }

        transform.position = pathPoints[waypointIndex].position;
    }

    
    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
        pathPoints = waveConfig.GetWaypoints();
        waypointIndex = 0;
    }

    void Update()
    {
        move();   
    }

    private void move()
    {
        if (pathPoints == null || pathPoints.Count == 0) return;

        waypointIndex = GetNextValidWaypointIndex(waypointIndex);
        if (waypointIndex == -1) return;

        var targetPosition = pathPoints[waypointIndex].position;
        var moveSpeed = waveConfig.GetMoveSpeed();
        var movementThisFrame = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementThisFrame);

        if (Vector3.Distance(transform.position, targetPosition) <= 0.001f)
        {
            waypointIndex++;
            if (waypointIndex > pathPoints.Count - 1)
            {
                waypointIndex = 0;
            }
        }
    }

    private int GetNextValidWaypointIndex(int startIndex)
    {
        if (pathPoints == null || pathPoints.Count == 0) return -1;

        for (int i = startIndex; i < pathPoints.Count; i++)
        {
            if (pathPoints[i] != null) return i;
        }

        return -1;
    }
}
