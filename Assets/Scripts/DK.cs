using System.Collections;
using UnityEngine;

public class DK : MonoBehaviour
{
    public GameObject heldBarrel;
    public GameObject barrelPrefab;
    public Transform barrelSpawnPoint;

    private Animator animator;

    public float attackAnimationLength = 1.5f;
    public float chestBeatLength = 3f;
    public float idleLength = 2f;

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

            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(attackAnimationLength);

            animator.SetTrigger("Chestbeat");
            yield return new WaitForSeconds(chestBeatLength);

            yield return new WaitForSeconds(idleLength);
        }
    }

    public void ShowBarrel()
{
    Debug.Log("DK: ShowBarrel called.");
    if (heldBarrel != null)
        heldBarrel.SetActive(true);
}

public void ThrowBarrel()
{
    Debug.Log("DK: ThrowBarrel called.");

    if (heldBarrel != null)
        heldBarrel.SetActive(false);

    if (barrelPrefab == null || barrelSpawnPoint == null)
    {
        Debug.LogError("DK: Missing barrelPrefab or barrelSpawnPoint.");
        return;
    }

    Instantiate(barrelPrefab, barrelSpawnPoint.position, barrelSpawnPoint.rotation);
    Debug.Log("Barrel spawned");
}
}