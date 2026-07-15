using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float boostedSpeed = 8f;
    public float boostDuration = 6f;
    public float jumpForce = 8f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    public object thePlayer;
    public Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private bool isFacingRight = true;
    private Animator myAnim;

    public bool canMove = true;
    public GameObject lightningEffects;
    public GameObject Shield_effect;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private LadderMovement ladderMovement; // no longer [SerializeField]

    private bool canTurn = true;

    void Start()
    {
        thePlayer = FindAnyObjectByType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        ladderMovement = GetComponent<LadderMovement>(); // grabbed automatically here
    }

    void Update()
    {
        if (!canTurn)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        bool isClimbing = ladderMovement != null && ladderMovement.isClimbing;

        myAnim.SetBool("IsClimbing", isClimbing);

        if (isClimbing)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            myAnim.speed = Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0f ? 1f : 0f;
            return;
        }
        else
        {
            myAnim.speed = 1f;
        }

        moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        
        if (canTurn == true)
        {
            myAnim.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
            if (moveInput > 0 && !isFacingRight)
            Flip();
        else if (moveInput < 0 && isFacingRight)
            Flip();
        }
        

        
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            whatIsGround
        );
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpeedBoots"))
        {
            Destroy(other.gameObject);
            StartCoroutine(SpeedBoost());
        }

        if (other.CompareTag("Peach"))
        {
           
            isFacingRight = false;
            canTurn = false;
            moveSpeed = 0f;
            myAnim.SetFloat("Speed", Mathf.Abs(0f));
        }
        
        if (other.CompareTag("Shield"))
        {
            Destroy(other.gameObject);
            StartCoroutine(Shield());
        }

        if (other.CompareTag("Barrel"))
        {
            myAnim.SetTrigger("Boom");
        }
    }

    IEnumerator SpeedBoost()
    {
        moveSpeed = boostedSpeed;
        lightningEffects.SetActive(true);
        yield return new WaitForSeconds(boostDuration);
        moveSpeed = 3f;
        lightningEffects.SetActive(false);

    }
    IEnumerator Shield()
    {
        moveSpeed = boostedSpeed;
        Shield_effect.SetActive(true);
        yield return new WaitForSeconds(boostDuration);
        moveSpeed = 3f;
    }

}