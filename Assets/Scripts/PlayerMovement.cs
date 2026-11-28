using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //public GameManager gameManager;
    [SerializeField] private PlayerAudio playerAudio;
    [SerializeField] private GameObject doubleJumpIcon;
    public enum PlayerState
    {
        Idle,
        Run,
        Jump,
        Fall,
        Dash,
        Slide,
        Crouch,
        Dead
    }
    Rigidbody2D rb;
    Vector2 movement;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float doubleJumpForce = 10f;
    private bool hasDoubleJumpPowerUp = false;
    private bool doubleJumpAvailable = false;


    /*
    [Header("Dash")]
    public float dashSpeed = 15;
    public float dashDuration = 0.18f;
    public float dashCooldown = 0.5f;
    public float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    */


    private PlayerState currentState = PlayerState.Idle;
    private PlayerState previousState = PlayerState.Idle;
    bool isGrounded = false;

    // Keep track of which platform the player is standing on
    private GameObject currentGround;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (doubleJumpIcon != null)
        {
            doubleJumpIcon.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Keyboard.current == null)
        {
            movement = Vector2.zero;
            return;
        }

        //A and D, left and right
        movement = Vector2.zero;

        if (Keyboard.current.aKey.isPressed)
        {
            movement += Vector2.left;
        }

        if (Keyboard.current.dKey.isPressed)
        {
            movement += Vector2.right;
        }

        movement = movement.normalized;

        //Jump
        bool jumped = false;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (isGrounded)
            {
                //Normal jump
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

                isGrounded = false;
                jumped = true;
            }
            else if (hasDoubleJumpPowerUp && doubleJumpAvailable)
            {
                //One extra jump while in the air
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

                rb.AddForce(Vector2.up * doubleJumpForce, ForceMode2D.Impulse);

                //One time only extra jump
                doubleJumpAvailable = false;

                if (doubleJumpIcon != null)
                {
                    doubleJumpIcon.SetActive(false);
                }

                jumped = true;
            }
        }

        UpdateState(jumped);
    }

    void UpdateState(bool jumped = false)
    {
        PlayerState nextState;

        if (jumped)
        {
            nextState = PlayerState.Jump;
        }
        else if (isGrounded)
        {
            nextState = movement.x == 0f ? PlayerState.Idle : PlayerState.Run;
        }
        else
        {
            nextState = rb.linearVelocity.y > 0f ? PlayerState.Jump : PlayerState.Fall;
        }

        if (nextState == currentState)
        {
            return;
        }

        previousState = currentState;
        currentState = nextState;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(
            movement.x * moveSpeed,
            rb.linearVelocity.y
            );
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player touches a platform
        if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // Only count landing on top of the platform
                if (contact.normal.y > 0.5f)
                {
                    isGrounded = true;

                    // Remember which platform we landed on
                    currentGround = collision.gameObject;

                    // Play the piano sound when landing
                    if (playerAudio != null)
                    {
                        playerAudio.PlayLanding(collision.gameObject);
                    }

                    break;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Only remove the grounded state when leaving
        // the platform the player was actually standing on
        if (collision.gameObject == currentGround)
        {
            isGrounded = false;
            currentGround = null;
        }
    }

    //Activate double jump power up
     public void ActivateDoubleJump()
    {
        if (!hasDoubleJumpPowerUp)
        {
            hasDoubleJumpPowerUp = true;
            doubleJumpAvailable = true;
            
            if (doubleJumpIcon != null)
            {
                doubleJumpIcon.SetActive(true);
            }
        }
    }

    //Detect power up
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DoubleJump"))
        {
            ActivateDoubleJump();

            //Remove collected power up
            Destroy(other.gameObject);
        }
    }
}
