using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Peach : MonoBehaviour
{
        private Animator animator;
        public GameObject spriteToShow;
        public VideoPlayer Explosion;
        public GameObject DKExplosion;
        private bool triggered = false;

    void Start()
    {
        animator = GetComponent<Animator>(); //get animator for peach
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
            DK DonkeyKong = FindAnyObjectByType<DK>();

        if (DonkeyKong != null)
        {
            DonkeyKong.StopDK();
        }
            CancelInvoke(nameof(PeachScream)); //stop screaming animation when player arrives
            animator.Play("Peach Idle");
            StartCoroutine(ShowSpriteAfterDelay());

            StartCoroutine(WinScreenAfterDelay());
           
        }
    }

    IEnumerator WinScreenAfterDelay() //delay before DK explosion plays
    {
        yield return new WaitForSeconds(3.0f);
        DKExplosion.SetActive (true);           //unhides DK exploding
        Explosion.Play();                       //DK explodes
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("Win Screen");
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
