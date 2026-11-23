using UnityEngine;

public class InvisiblePlatforms : MonoBehaviour
{
    // Player reference and distance to reveal the platform
    public Transform player;
    public float revealDistance = 3f;

    private SpriteRenderer platformRenderer;

    // Check if the platform has been touched
    private bool hasBeenTouched = false;
    private bool playerTouching = false;

    // Check if the entire level is being revealed
    private bool showingOverview = false;

    void Start()
    {
        // Get the renderer and hide the platform
        platformRenderer = GetComponent<SpriteRenderer>();
        platformRenderer.enabled = false;
    }

    void Update()
    {
        // Skip distance checks during the level reveal
        if (showingOverview || player == null || hasBeenTouched)
            return;

        // Check the distance between the player and platform
        float distance = Vector2.Distance(
            player.position, transform.position
        );

        // Reveal the platform when the player is nearby
        platformRenderer.enabled = distance <= revealDistance;
    }

    // Called by the reveal platform
    public void SetOverview(bool show)
    {
        showingOverview = show;

        // Keep the platform visible if the player is touching it
        platformRenderer.enabled = show || playerTouching;

        // After the reveal, keep it hidden until touched
        if (!show)
        {
            hasBeenTouched = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Reveal the platform when the player lands on it
        if (collision.gameObject.CompareTag("Player"))
        {
            playerTouching = true;
            hasBeenTouched = true;
            platformRenderer.enabled = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the player leaves the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            playerTouching = false;

            // Hide the platform unless the level is revealed
            if (!showingOverview)
            {
                platformRenderer.enabled = false;
            }
        }
    }
}
