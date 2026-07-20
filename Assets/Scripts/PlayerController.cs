using System.Collections; // Needed for IEnumerator and coroutines.
using Unity.VisualScripting; // Kept because your project already uses it.
using UnityEngine; // Gives access to MonoBehaviour, Rigidbody2D, Animator, etc.
using UnityEngine.SceneManagement; // Lets us reload the scene when the player dies.

public class PlayerController : MonoBehaviour // This script controls the player character.
{
    public float moveSpeed = 3f; // Normal movement speed.
    public float boostedSpeed = 6f; // Speed during the speed boost power-up.
    public float boostDuration = 4f; // How long the speed boost lasts.
    public float jumpForce = 8f; // How high the player jumps.
    public float hammerDuration = 4f; //duration of hammer

    public Transform groundCheck; // A point below the player used to check if we are standing on ground.
    public float groundCheckRadius = 0.2f; // Radius of the ground-check circle.
    public LayerMask whatIsGround; // Which layers count as ground.

    public object thePlayer; // Existing reference in your script.
    public Rigidbody2D rb; // The player's physics body.
    private float moveInput; // Stores left/right input.
    private bool isGrounded; // True when the player is on the ground.
    private bool isFacingRight = true; // Tracks the direction the player is facing.
    private Animator myAnim; // Controls player animations.

    public bool canMove = true; // Lets you disable movement later if needed.
    public GameObject lightningEffects; // Visual effect for the speed boost.
    public GameObject Shield_effect; // The shield sprite / shield visual object.
    [SerializeField] private SpriteRenderer spriteRenderer; // The player's visible sprite so we can blink it.
    private LadderMovement ladderMovement; // Reference to the ladder script.

    private bool canTurn = true; // Controls whether the player can flip direction.

    public bool shieldActive = false; // True while the one-hit shield is available.
    private int shieldHits = 0; // How many hits the shield can still absorb.

    private Coroutine blinkRoutine; // Stores the blinking coroutine so we can stop it later.
    public float blinkInterval = 0.15f; // Speed of the blinking effect.

    public GameObject HammerHitbox;

    void Start() // Runs once when the scene starts.
    {
        thePlayer = FindAnyObjectByType<PlayerController>(); // Finds the player object in the scene.
        rb = GetComponent<Rigidbody2D>(); // Gets the Rigidbody2D on this object.
        myAnim = GetComponent<Animator>(); // Gets the Animator on this object.
        ladderMovement = GetComponent<LadderMovement>(); // Gets the ladder script if it exists.

        if (spriteRenderer == null) // If the sprite renderer was not assigned in the Inspector...
            spriteRenderer = GetComponent<SpriteRenderer>(); // Grab it from this same object.

        if (Shield_effect != null) // Make sure the shield object exists before touching it.
            Shield_effect.SetActive(false); // Start with the shield hidden.

            HammerHitbox.SetActive(false);
    }

    void Update() // Runs once every frame.
    {
        if (!canTurn) // If turning is disabled...
        {
            rb.linearVelocity = Vector2.zero; // Stop the player completely.
            return; // Skip the rest of Update.
        }

        bool isClimbing = ladderMovement != null && ladderMovement.isClimbing; // Check if the player is climbing.
        myAnim.SetBool("IsClimbing", isClimbing); // Tell the Animator if we are climbing.

        if (isClimbing) // If the player is on a ladder...
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y); // Stop horizontal movement while climbing.
            myAnim.speed = Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0f ? 1f : 0f; // Pause or play climb animation.
            return; // Do not run normal walking code while climbing.
        }
        else
        {
            myAnim.speed = 1f; // Make sure animations play normally when not climbing.
        }

        moveInput = Input.GetAxisRaw("Horizontal"); // Read left/right movement input.
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y); // Move the player sideways.

        if (Input.GetButtonDown("Jump") && isGrounded) // Jump only if the player is grounded.
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Push the player upward.
        }

        if (canTurn == true) // Only flip the player if turning is allowed.
        {
            myAnim.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x)); // Update the speed parameter in the Animator.
            if (moveInput > 0 && !isFacingRight) // Moving right but currently facing left...
                Flip(); // Turn to face right.
            else if (moveInput < 0 && isFacingRight) // Moving left but currently facing right...
                Flip(); // Turn to face left.
        }
    }

    void FixedUpdate() // Runs at a fixed time step, which is better for physics checks.
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            whatIsGround
        ); // Detect whether the ground-check circle overlaps the ground layer.
    }

    void Flip() // Flips the player sprite left/right.
    {
        isFacingRight = !isFacingRight; // Switch the facing direction.

        Vector3 scaler = transform.localScale; // Get the current local scale.
        scaler.x *= -1; // Reverse the X scale to mirror the sprite.
        transform.localScale = scaler; // Apply the new scale.
    }

    private void OnTriggerEnter2D(Collider2D other) // Runs when the player enters a trigger collider.
    {
        if (other.CompareTag("SpeedBoots")) // If the player touches a speed boost pickup...
        {
            Destroy(other.gameObject); // Remove the pickup.
            StartCoroutine(SpeedBoost()); // Start the speed boost coroutine.
        }

        if (other.CompareTag("Peach")) // If the player touches Peach...
        {
            isFacingRight = false; // Force facing direction.
            canTurn = false; // Stop the player from turning.
            moveSpeed = 0f; // Freeze movement.
            myAnim.SetFloat("Speed", Mathf.Abs(0f)); // Set animation speed to zero.
        }

        if (other.CompareTag("Shield")) // If the player touches a shield pickup...
        {
            Destroy(other.gameObject); // Remove the pickup.
            StartCoroutine(Shield()); // Start the one-hit shield coroutine.
        }

        if (other.CompareTag("Invincibility")) // If the player touches the invincibility pickup...
        {
            Destroy(other.gameObject); // Remove the pickup.
            StartCoroutine(InvincibilityPowerUp()); // Start the blinking invincibility effect.
        }

        if (other.CompareTag("Hammer"))
        {
          
            Destroy(other.gameObject);
            StartCoroutine(HammerPower());
        }
    }

    IEnumerator SpeedBoost() // Makes the player faster for a limited time.
    {
        moveSpeed = boostedSpeed; // Increase movement speed.
        lightningEffects.SetActive(true); // Show the lightning effect.
        yield return new WaitForSeconds(boostDuration); // Wait until the boost ends.
        moveSpeed = 3f; // Restore normal movement speed.
        lightningEffects.SetActive(false); // Hide the lightning effect.
    }
    
    IEnumerator Shield() // Activates a shield that blocks one hit.
    {
        shieldActive = true; // Turn shield protection on.
        shieldHits = 1; // The shield can absorb exactly one hit.

        if (Shield_effect != null) // Make sure the shield visual exists.
            Shield_effect.SetActive(true); // Show the shield sprite.

        yield return new WaitForSeconds(boostDuration); // Keep the shield active for the timer.

        if (shieldActive) // If the shield was not already consumed by a hit...
        {
            shieldActive = false; // Turn shield protection off.
            shieldHits = 0; // Reset hit count.

            if (Shield_effect != null) // Make sure the shield visual exists.
                Shield_effect.SetActive(false); // Hide the shield sprite.
        }
    }

    IEnumerator InvincibilityPowerUp() // Makes the player blink for a short time.
    {
        if (blinkRoutine != null) // If blinking is already running...
            StopCoroutine(blinkRoutine); // Stop the old blink coroutine.

        blinkRoutine = StartCoroutine(BlinkPlayer()); // Start blinking the player sprite.

        yield return new WaitForSeconds(5f); // Keep invincibility active for 5 seconds.

        if (blinkRoutine != null) // If the blink coroutine is still running...
            StopCoroutine(blinkRoutine); // Stop it.

        if (spriteRenderer != null) // Make sure the player sprite exists.
            spriteRenderer.enabled = true; // Force the sprite visible again at the end.
    }

    IEnumerator BlinkPlayer() // Makes the sprite flicker on and off.
    {
        while (true) // Keep blinking until the coroutine is stopped.
        {
            if (spriteRenderer != null) // Check that the sprite exists.
                spriteRenderer.enabled = false; // Hide the sprite.

            yield return new WaitForSeconds(blinkInterval); // Wait a short moment.

            if (spriteRenderer != null) // Check again before re-enabling.
                spriteRenderer.enabled = true; // Show the sprite.

            yield return new WaitForSeconds(blinkInterval); // Wait again before hiding it.
        }
    }

    IEnumerator HammerPower()
    {
        myAnim.SetBool("Hammer", true);
        HammerHitbox.SetActive(true) ;
        yield return new WaitForSeconds(hammerDuration);

        HammerHitbox.SetActive(false);
        myAnim.SetBool("Hammer", false);
    }

    public void TakeHit() // Call this when a barrel hits the player.
    {
        if (shieldActive) // If the shield is active...
        {
            shieldHits--; // Spend one shield hit.

            if (shieldHits <= 0) // If the shield has no hits left...
            {
                shieldActive = false; // Turn shield protection off.

                if (Shield_effect != null) // Make sure the shield visual exists.
                    Shield_effect.SetActive(false); // Hide the shield sprite.
            }

            return; // Do not restart the scene because the shield blocked the hit.
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart the current scene if no shield is left.
    }
}