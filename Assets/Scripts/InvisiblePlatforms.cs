using UnityEngine;

public class InvisiblePlatforms : MonoBehaviour
{
    // Player reference and distance to reveal the platform
    public Transform player;
    public float revealDistance = 3f;

    private MeshRenderer platformRenderer;

    // Check if the player has touched the platform before
    private bool hasBeenTouched = false;

    void Start()
    {
        // Get the MeshRenderer and hide the platform
        platformRenderer = GetComponent<MeshRenderer>();
        platformRenderer.enabled = false;
    }

    void Update()
    {
        // Stop checking distance after the player first touches it
        if (player == null || hasBeenTouched)
            return;

        // Check how close the player is to the platform
        float distance = Vector2.Distance(
            player.position, transform.position
        );

        // Show the platform when the player is nearby
        platformRenderer.enabled = distance <= revealDistance;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Show the platform when the player touches it
        if (collision.gameObject.CompareTag("Player"))
        {
            hasBeenTouched = true;
            platformRenderer.enabled = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Hide the platform when the player leaves it
        if (collision.gameObject.CompareTag("Player"))
        {
            platformRenderer.enabled = false;
        }
    }
}
