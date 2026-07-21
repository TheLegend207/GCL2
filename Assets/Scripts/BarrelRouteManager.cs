using System.Collections.Generic;
using UnityEngine;

public class BarrelRouteManager : MonoBehaviour
{
    public static BarrelRouteManager Instance;

    [Header("Route Variations")]
    [SerializeField] private List<Transform> route1 = new List<Transform>(); //route 1 for barrels
    [SerializeField] private List<Transform> route2 = new List<Transform>(); // route 2 for barrels
    [SerializeField] private List<Transform> route3 = new List<Transform>(); // route 3 for barrels

    private void Awake()
    {
        Instance = this;
    }

    public List<Transform> GetRandomRoute() //choose a random path for barrels to travel down
    {
        int choice = Random.Range(0, 3);

        if (choice == 0) return route1;
        if (choice == 1) return route2;
        return route3;
    }
}