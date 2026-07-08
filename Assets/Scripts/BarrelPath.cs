using System.Collections.Generic;
using UnityEngine;

public class BarrelPath : MonoBehaviour
{
    private List<Transform> waypoints;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float stopDistance = 0.1f;
    private int currentWaypointIndex = 0;

    void Update()
    {
        if (waypoints == null || waypoints.Count == 0)
            return;

        if (currentWaypointIndex >= waypoints.Count)
            return;

        Transform target = waypoints[currentWaypointIndex];

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) <= stopDistance)
        {
            currentWaypointIndex++;
        }
    }

    public void SetPath(List<Transform> newPath)
    {
        waypoints = newPath;
        currentWaypointIndex = 0;
    }
}