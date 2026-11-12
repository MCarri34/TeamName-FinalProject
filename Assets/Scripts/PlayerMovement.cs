using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 movement;

    public float moveSpeed = 5f;
    public float jumpForce = 7f;

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
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
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
        if (other.CompareTag("Ground"))
        {
            isGrounded = true;
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
