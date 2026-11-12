using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //public GameManager gameManager;
    [SerializeField] private PlayerAudio playerAudio;
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        bool jumped = Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded;
        if (jumped)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
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

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ground") && rb.linearVelocityY <= 0)
        {
            bool justLanded = !isGrounded;
            isGrounded = true;

            if (justLanded && playerAudio != null)
            {
                playerAudio.PlayLanding(other.gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Ground"))
            {
                isGrounded = false;
            }
        }
}
