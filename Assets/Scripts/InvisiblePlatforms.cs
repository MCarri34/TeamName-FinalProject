using UnityEngine;

public class InvisiblePlatforms : MonoBehaviour
{
    // Player reference and distance to reveal the platform
    public Transform player;
    public float revealDistance = 3f;

    private SpriteRenderer platformRenderer;

    // Check if the platform has been touched
    private bool hasBeenTouched = false;

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

    // Called by the special platform
    public void SetOverview(bool show)
    {
        showingOverview = show;

        // Show or hide the platform
        platformRenderer.enabled = show;

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
            hasBeenTouched = true;
            platformRenderer.enabled = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Hide the platform when the player leaves it
        if (collision.gameObject.CompareTag("Player") && !showingOverview)
        {
            platformRenderer.enabled = false;
        }
    }
}
