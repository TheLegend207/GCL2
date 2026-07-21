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
    public float tripleAttackLength = 4.5f;
    public bool hardMode;
    //edit animation length timing in unity instead of in the code
    //to permenantly change length set aniamtionLength = X under void start 

    private Coroutine dkPatternCoroutine;


    void Start()
    {
        animator = GetComponent<Animator>(); //find the animator for DK
        dkPatternCoroutine = StartCoroutine(DKPattern()); //start the cycle of DK animation
        
    }

    IEnumerator DKPattern() //DK attack patterns
    {
        while (true)
        {
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(attackAnimationLength);

            animator.SetTrigger("Chestbeat");
            yield return new WaitForSeconds(chestBeatLength);

            animator.SetTrigger("Idle");
            yield return new WaitForSeconds(idleLength);

            if (hardMode == true)
            {
                animator.SetTrigger("Attack");
                yield return new WaitForSeconds(tripleAttackLength);

                animator.SetTrigger("Chestbeat");
                yield return new WaitForSeconds(chestBeatLength);

                animator.SetTrigger("Idle");
                yield return new WaitForSeconds(idleLength);
            }
        }
    }
        public void ShowBarrel()
{
    if (heldBarrel != null)
        heldBarrel.SetActive(true); //show barrel before being thrown
}
     public void ThrowBarrel()
{
    if (heldBarrel != null)
        heldBarrel.SetActive(false); //hide barrel after being thrown

    if (barrelPrefab == null || barrelSpawnPoint == null) //destroy barrel if null
        return;

    GameObject barrel = Instantiate(barrelPrefab, barrelSpawnPoint.position, barrelSpawnPoint.rotation); //spawn barrel position

    BarrelPath barrelPath = barrel.GetComponent<BarrelPath>(); //gets barrel path from other script
    if (barrelPath != null && BarrelRouteManager.Instance != null)
    {
        List<Transform> route = BarrelRouteManager.Instance.GetRandomRoute(); //get a random route from other script
        if (route != null && route.Count > 0)
        {
            barrelPath.SetPath(route); //set path to the random route selected
        }
    }
}

public void StopDK()
    {
        if(dkPatternCoroutine != null)
        {
            StopCoroutine(dkPatternCoroutine); //stop animations when player reaches peach
        }

        animator.SetTrigger("Idle"); //set animation to idle animation
    }
}