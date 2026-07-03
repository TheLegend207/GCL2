using System.Collections;
using UnityEngine;

public class Peach : MonoBehaviour
{
        private Animator animator;
        public GameObject spriteToShow;
        private bool triggered = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        InvokeRepeating(nameof(PeachScream), 8f, 8f); //Plays the scream animation every 8 seconds
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player")) //using reference to when player arrive in peach's hitbox
        {
            triggered = true;
            CancelInvoke(nameof(PeachScream)); //stop screaming animation when player arrives
            animator.Play("Peach Idle");
            StartCoroutine(ShowSpriteAfterDelay());
        }
    }

    IEnumerator ShowSpriteAfterDelay()
    {
        yield return new WaitForSeconds(1.5f); //just wait for 1.5 seconds before showing the heart
        spriteToShow.SetActive(true);
    }

    void PeachScream()
    {
        animator.SetTrigger("Scream");
    }

}
