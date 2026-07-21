using System.Collections.Generic;
using UnityEngine;

public class BarrelPath : MonoBehaviour
{
    private List<Transform> waypoints; //creates waypoints for barrels to go to
    [SerializeField] private float moveSpeed = 3f; //barrel speed
    [SerializeField] private float stopDistance = 0.1f; //barrel stops moving distance
    private int currentWaypointIndex = 0;

    void Update()
    {
        if (waypoints == null || waypoints.Count == 0) //return if no waypoints
            return; 

        if (currentWaypointIndex >= waypoints.Count) //destroy when reaching past last waypoint
            return;

        Transform target = waypoints[currentWaypointIndex]; //position and count of waypoints

        transform.position = Vector3.MoveTowards( //moves the barrel towards the next waypoint
            transform.position,
            target.position, //waypoints position
            moveSpeed * Time.deltaTime //speed and time of barrel movement
        );

        if (Vector3.Distance(transform.position, target.position) <= stopDistance)
        {
            currentWaypointIndex++; //increase waypoint count when reaching a new waypoint
        }
    }

    public void SetPath(List<Transform> newPath)
    {
        waypoints = newPath;
        currentWaypointIndex = 0;
    }
}