using System.Collections;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f; // normal movement speed
    public float boostedSpeed = 6f; // speed boost speed
    public float boostDuration = 4f; // power up duration
    public float jumpForce = 5.2f; // player jump height
    public float hammerDuration = 4f; //duration of hammer
    public float invincibilityTime = 5f; //time for invincibility

    public Transform groundCheck; // checks if player is on ground
    public float groundCheckRadius = 0.2f; // Radius of the ground-check circle.
    public LayerMask whatIsGround; // Which layers count as ground.

    public object thePlayer;  //references the player
    public Rigidbody2D rb; //finds rigidbody2d
    private float moveInput; 
    private bool isGrounded; 
    private bool isFacingRight = true; 
    private Animator myAnim; //finds animator
    public GameObject lightningEffects; 
    public GameObject Shield_effect; 
    [SerializeField] private SpriteRenderer spriteRenderer; 
    private LadderMovement ladderMovement;  //finds ladder script

    private bool canTurn = true; 
    public bool canClimb; 
    public bool isInvincible = false; 
    public bool shieldActive = false; 
    private int shieldHits = 0; 

    private Coroutine blinkRoutine;
    public float blinkInterval = 0.15f; 

    public GameObject HammerHitbox;


    void Start() 
    {
        thePlayer = FindAnyObjectByType<PlayerController>(); // finds player controller
        rb = GetComponent<Rigidbody2D>(); // finds rigidbody2d
        myAnim = GetComponent<Animator>(); // finds player animator
        ladderMovement = GetComponent<LadderMovement>(); // finds ladder script
        canClimb = true;

        if (spriteRenderer == null) 
            spriteRenderer = GetComponent<SpriteRenderer>();  //get sprite from object if sprite is null

        if (Shield_effect != null) 
            Shield_effect.SetActive(false);  //start with shield hidden

            HammerHitbox.SetActive(false); //start with hammer hitbox hidden
    }

    void Update() 
    {
        if (!canTurn) 
        {
            rb.linearVelocity = Vector2.zero; //player cannot turn if it is on
            return; 
        }

        bool isClimbing = ladderMovement != null && ladderMovement.isClimbing; 
        myAnim.SetBool("IsClimbing", isClimbing);  //set animation to climbing

        if (isClimbing) //when climbing
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y); //stops horizontal movement
            myAnim.speed = Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0f ? 1f : 0f; 
            return;
        }
        else
        {
            myAnim.speed = 1f; //makes sure animation speed is 1f
        }

        moveInput = Input.GetAxisRaw("Horizontal"); //gets left right input
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y); //player moves left right

        if (Input.GetButtonDown("Jump") && isGrounded) // jump if the player is grounded
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // how high the player jumps
        }

        if (canTurn == true) // player can flip if true
        {
            myAnim.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x)); 
            if (moveInput > 0 && !isFacingRight) 
                Flip(); // flip to the right
            else if (moveInput < 0 && isFacingRight) 
                Flip(); // flip to the left
        }
    }

    void FixedUpdate() 
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            whatIsGround
        ); //checks if groundcheck is in contact with ground layer
    }

    void Flip() //flips player left or right
    {
        isFacingRight = !isFacingRight;

        Vector3 scaler = transform.localScale; //get current scale
        scaler.x *= -1; //flip current scale
        transform.localScale = scaler;  //set new scale as the "current" scale
    }

    private void OnTriggerEnter2D(Collider2D other) //when player enters trigger 2d
    {
        if (other.CompareTag("SpeedBoots")) //if tag is for speedboosts
        {
            Destroy(other.gameObject); //destroy speedboots
            StartCoroutine(SpeedBoost()); //speed up the player
        }

        if (other.CompareTag("Peach")) //if tag is for peach
        {
            isFacingRight = false; //make player look left
            canTurn = false; // stop the player from turning
            moveSpeed = 0f; // stops player from moving left or right
            myAnim.SetFloat("Speed", Mathf.Abs(0f)); // set animation speed to zero
        }

        if (other.CompareTag("Shield")) // if tag is shield
        {
            Destroy(other.gameObject); // destroy shield
            StartCoroutine(Shield()); // do shield coroutine
        }

        if (other.CompareTag("Invincibility")) // if tag is invincibility
        {
            Destroy(other.gameObject); // remove the pickup
            StartCoroutine(InvincibilityPowerUp()); // start invincibility effect coroutine
        }

        if (other.CompareTag("Hammer")) //if tag is hammer
        {
          
            Destroy(other.gameObject); //remove hammer pickup
            StartCoroutine(HammerPower()); //do the hammer coroutine
        }
    }

    IEnumerator SpeedBoost() 
    {
        moveSpeed = boostedSpeed; // increase movement speed
        lightningEffects.SetActive(true); // show the lightning
        yield return new WaitForSeconds(boostDuration); // time to wait until boost eneds
        moveSpeed = 3f; // return to base speed
        lightningEffects.SetActive(false); // hide the lightning
    }
    
    IEnumerator Shield() 
    {
        shieldActive = true; // turn on shield
        shieldHits = 1; // set shield hp to 1

        if (Shield_effect != null) 
            Shield_effect.SetActive(true); //show shield sprite if not shown already

        yield return new WaitForSeconds(boostDuration); // shield timer

        if (shieldActive) // if shield isnt used
        {
            shieldActive = false; // turn off shield
            shieldHits = 0; // set hp to 0

            if (Shield_effect != null) 
                Shield_effect.SetActive(false); //hide the shield
        }
    }

    IEnumerator InvincibilityPowerUp() 
    {
        isInvincible = true; // turn on invincibility

        if (blinkRoutine != null) 
            StopCoroutine(blinkRoutine); 

        blinkRoutine = StartCoroutine(BlinkPlayer()); //blink the player

        yield return new WaitForSeconds(invincibilityTime); // invincibility powerup duration

         isInvincible = false; // turn off invincibility

        if (blinkRoutine != null) 
            StopCoroutine(blinkRoutine); // force stop coroutine

        if (spriteRenderer != null) 
            spriteRenderer.enabled = true; //force sprite renderer if it is not loaded
    }

    IEnumerator BlinkPlayer() //make player flicker
    {
        while (true) //loop to keep going until turned off
        {
            if (spriteRenderer != null) 
                spriteRenderer.enabled = false; //hide sprite

            yield return new WaitForSeconds(blinkInterval); // wait some time

            if (spriteRenderer != null) 
                spriteRenderer.enabled = true; //show sprite

            yield return new WaitForSeconds(blinkInterval); // wait some time before looping
        }
    }

    IEnumerator HammerPower()
    {
        canClimb = false; //force player unable to climb
        jumpForce = 0f;//force player unable to jump
        myAnim.SetBool("Hammer", true); //turn on hammer swing animation
        HammerHitbox.SetActive(true) ; //turn on hammer hitbox
        yield return new WaitForSeconds(hammerDuration); //wait time for hammer duration

        HammerHitbox.SetActive(false); //turn off hammer hitbox
        myAnim.SetBool("Hammer", false); //turn off swing animation
        canClimb = true; //let player climb again
        jumpForce = 5.2f; //let player jump again
    }

    public void Die() //dies when player is hit 
    {
        if (shieldActive) //if shield is active
        {
            shieldHits--; //-1hp from shield

            if (shieldHits <= 0) // if shield 0hp
            {
                shieldActive = false; // turn off shield

                if (Shield_effect != null) 
                    Shield_effect.SetActive(false); // force hide shield sprite
            }

            return; 
        }
        {
        if (isInvincible) //if invincibility is on
            return; // ignore shield damage

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        moveSpeed = 0f;
        canTurn = false;


        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}