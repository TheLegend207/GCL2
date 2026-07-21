using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    private float vertical;
    private float climbSpeed = 3f;
    private bool isLadder;
    public PlayerController player;
    public bool canClimb;

    // must be public (or public property) so PlayerController can read it
    public bool isClimbing { get; private set; }

    [SerializeField] private Rigidbody2D rb;

    private void Start()
    {
        player = GetComponent<PlayerController>(); //find player controller
    }

    void Update()
    {
        if (player.canClimb == true) //if canClimb bool from player controller is true
        {
            vertical = Input.GetAxisRaw("Vertical"); //get input for up/down

            if (isLadder && Mathf.Abs(vertical) > 0f)
            {
                isClimbing = true;
                rb.gravityScale = 0f; //pplayer does not move down and stays on ladder
            }
            else if (!isLadder || Mathf.Abs(vertical) == 0f)
            {
                isClimbing = false;
                rb.gravityScale = 1f; //player has gravity again and moves down
            }
        }
    }

    private void FixedUpdate()
    {
        if (isClimbing) //currently climbing
        {
            rb.gravityScale = 0f; //player does not fall down
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * climbSpeed); //set speed for climbing up
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //when colliding with a trigger
    {
        if (collision.CompareTag("Ladder")) //check if other hitbox is ladder
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) //when leaving a trigger hitbox
    {
        if (collision.CompareTag("Ladder")) //check if the other hitbox is ladder
        {
            isLadder = false;
            isClimbing = false;
            rb.gravityScale = 1f; //player has gravity again
        }
    }
}