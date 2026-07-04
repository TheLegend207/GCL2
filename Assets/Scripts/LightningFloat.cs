using System.Collections;
using UnityEngine;

public class LightningFloat : MonoBehaviour
{
    public float moveDistance = 0.4f;
    public float switchTime = 0.15f;
    public bool startUp = true; 
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition; //save lightning start position
        StartCoroutine(Teleport());
    }

    void Update()
    {
        
    }

    IEnumerator Teleport()
    {
        bool up = startUp; //to prevent both lightning move same direction

    while (true) //animation for going up and going down respectively before switching direction
    {
        if (up)
            transform.localPosition = startPos + Vector3.up * moveDistance;
        else
            transform.localPosition = startPos - Vector3.up * moveDistance;

        up = !up;

        yield return new WaitForSeconds(switchTime);
    }

    }
}
