using System.Collections;
using UnityEngine;

public class DK : MonoBehaviour
{
    public GameObject heldBarrel;
    private Animator animator;
    public float attackAnimationLength = 1.5f; //the floats just for how long each animation is
    public float chestBeatLength = 3f;
    public float idleLength = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(DKPattern());
    }

    IEnumerator DKPattern() //DK attack pattern consist of two barrel throws, one to beat its chest and the last to just idle for a bit before repeating the cycle
    {
        while (true)
        {
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(attackAnimationLength);

            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(attackAnimationLength);

            animator.SetTrigger("Chestbeat");
            yield return new WaitForSeconds(chestBeatLength);

            yield return new WaitForSeconds(idleLength);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowBarrel() //For setting up animation event to make barrel disappear and reappear
    {
        heldBarrel.SetActive(true);
    }

    public void ThrowBarrel()
    {
        heldBarrel.SetActive(false);
    }
}
