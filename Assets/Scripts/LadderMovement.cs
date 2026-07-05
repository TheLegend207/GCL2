using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    private float vertical;
    private float climbSpeed = 3f;
    private bool isLadder;

    // must be public (or public property) so PlayerController can read it
    public bool isClimbing { get; private set; }

    [SerializeField] private Rigidbody2D rb;

    void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");

        if (isLadder && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true;
            rb.gravityScale = 0f;
        }
        else if (!isLadder || Mathf.Abs(vertical) == 0f)
        {
            isClimbing = false;
            rb.gravityScale = 1f;
        }
    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * climbSpeed);
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
            rb.gravityScale = 1f;
        }
    }
}