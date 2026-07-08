using System.Collections.Generic;
using UnityEngine;

public class BarrelPath : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float stopDistance = 0.1f;

    private int currentWaypointIndex = 0;

    void Update()
    {
        if (waypoints == null || waypoints.Count == 0)
            return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetWaypoint.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetWaypoint.position) <= stopDistance)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Count)
                currentWaypointIndex = waypoints.Count - 1;
        }
    }
}