using System.Collections.Generic;
using UnityEngine;

public class PathHolder : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints = new List<Transform>();

    public List<Transform> GetWaypoints()
    {
        return waypoints;
    }
}