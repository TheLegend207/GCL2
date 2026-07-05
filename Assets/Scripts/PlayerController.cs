using System.Collections;
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
    [SerializeField] private SpriteRenderer spriteRenderer;
    private LadderMovement ladderMovement; // no longer [SerializeField]

    void Start()
    {
        thePlayer = FindAnyObjectByType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        ladderMovement = GetComponent<LadderMovement>(); // grabbed automatically here
    }

    void Update()
    {
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

        moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (moveInput > 0 && !isFacingRight)
            Flip();
        else if (moveInput < 0 && isFacingRight)
            Flip();

        myAnim.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
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
    }

    IEnumerator SpeedBoost()
    {
        moveSpeed = boostedSpeed;
        lightningEffects.SetActive(true);
        yield return new WaitForSeconds(boostDuration);
        moveSpeed = 3f;
        lightningEffects.SetActive(false);
    }
}