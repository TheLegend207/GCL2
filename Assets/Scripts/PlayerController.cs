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

    // public LevelManager theLevelManager; (tba)
    public bool canMove = true;
    public GameObject lightningEffects;
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Start()
    {
        thePlayer = FindAnyObjectByType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput*moveSpeed, rb.linearVelocity.y);

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
        if (other.CompareTag("SpeedBoots")) //power-up speed boots give player speed and destroy itself
        {
            Destroy(other.gameObject);
            StartCoroutine(SpeedBoost());
        }
    }

    IEnumerator SpeedBoost() //increase movement speed temporarily and have effects beside it before going back to original speed
    {
        moveSpeed = boostedSpeed;
        lightningEffects.SetActive(true);
        yield return new WaitForSeconds(boostDuration);
        moveSpeed = 3f;
        lightningEffects.SetActive(false);
    }

}