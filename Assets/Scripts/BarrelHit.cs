using UnityEngine; // Gives access to Unity classes like MonoBehaviour and Collider2D.
using System.Collections; // Needed for IEnumerator and coroutines.

public class BarrelHit : MonoBehaviour // Controls barrel hit behavior.
{
    public int scoreValue; // How many points this barrel gives when destroyed.
    private LevelManager theLevelManager; // Reference to the level manager.

    public Animator animator; // Animator used to play the barrel destroy animation.
    public float destroyDelay = 0.35f; // How long to wait before destroying the barrel.
    private bool isDestroying = false; // Prevents the destroy sequence from running more than once.

    void Start() // Runs once when the object starts.
    {
        theLevelManager = FindFirstObjectByType<LevelManager>(); // Finds the LevelManager in the scene.

        if (animator == null) // If no Animator was assigned in the Inspector...
            animator = GetComponent<Animator>(); // Grab the Animator from this same GameObject.
    }

    private void OnTriggerEnter2D(Collider2D other) // Runs when the barrel hits a trigger collider.
    {
        if (isDestroying) // If the barrel is already being destroyed...
            return; // Do nothing.

        PlayerController player = other.GetComponent<PlayerController>(); // Try to get the player script from the object we hit.

        if (player != null) // If the barrel hit the player...
        {
            player.Die(); // Kill the player.
        }

        if (other.CompareTag("Hammer Hitbox")) // If the barrel got hit by the hammer...
        {
            StartCoroutine(PlayDestroyAnimation()); // Play the barrel destroy animation first.
            theLevelManager.AddScore(scoreValue); // Add score for destroying the barrel.
        }
    }

    IEnumerator PlayDestroyAnimation() // Plays the destroy animation, then removes the barrel.
    {
        isDestroying = true; // Mark the barrel as already being destroyed.
        animator.SetTrigger("Destroy"); // Tell the Animator to play the destroy animation.
        yield return new WaitForSeconds(destroyDelay); // Wait for the animation to play.
        Destroy(gameObject); // Remove the barrel from the scene.
    }
}