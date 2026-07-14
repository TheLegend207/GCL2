using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DK : MonoBehaviour
{
    public GameObject heldBarrel;
    public GameObject barrelPrefab;
    public Transform barrelSpawnPoint;

    private Animator animator;

    public float attackAnimationLength = 3f;
    public float chestBeatLength = 1.5f;
    public float idleLength = 1.5f;
    //edit animation length timing in unity instead of in the code
    //to permenantly change length set aniamtionLength = X under void start

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(DKPattern());
        
    }

    IEnumerator DKPattern()
    {
        while (true)
        {
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(attackAnimationLength);

           /* animator.SetTrigger("Attack");
            yield return new WaitForSeconds(attackAnimationLength);  */

            animator.SetTrigger("Chestbeat");
            yield return new WaitForSeconds(chestBeatLength);

            animator.SetTrigger("Idle");
            yield return new WaitForSeconds(idleLength);
        }
    }
        public void ShowBarrel()
{
    if (heldBarrel != null)
        heldBarrel.SetActive(true);
}
     public void ThrowBarrel()
{
    if (heldBarrel != null)
        heldBarrel.SetActive(false);

    if (barrelPrefab == null || barrelSpawnPoint == null)
        return;

    GameObject barrel = Instantiate(barrelPrefab, barrelSpawnPoint.position, barrelSpawnPoint.rotation);

    BarrelPath barrelPath = barrel.GetComponent<BarrelPath>();
    if (barrelPath != null && BarrelRouteManager.Instance != null)
    {
        List<Transform> route = BarrelRouteManager.Instance.GetRandomRoute();
        if (route != null && route.Count > 0)
        {
            barrelPath.SetPath(route);
        }
    }
}
}