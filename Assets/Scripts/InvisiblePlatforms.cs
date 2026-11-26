using UnityEngine;

public class InvisiblePlatforms : MonoBehaviour
{
    private SpriteRenderer platformRenderer;

    // Particle effect when the player touches the platform
    [SerializeField] private ParticleSystem revealParticle;

    void Awake()
    {
        // Get the platform's SpriteRenderer
        platformRenderer = GetComponent<SpriteRenderer>();

        // Keep the renderer enabled
        platformRenderer.enabled = true;

        // Only show the platform inside the player's mask
        SetOverview(false);
    }

    // Called when entering or leaving the reveal area
    public void SetOverview(bool show)
    {
        if (show)
        {
            // Show the entire platform
            platformRenderer.maskInteraction =
                SpriteMaskInteraction.None;
        }
        else
        {
            // Only show the area surrounding the player
            platformRenderer.maskInteraction =
                SpriteMaskInteraction.VisibleInsideMask;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Play particles when the player touches the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            if (revealParticle != null)
            {
                revealParticle.Play(true);
            }
        }
    }
}
