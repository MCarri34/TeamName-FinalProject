using UnityEngine;

public class RevealPlatform : MonoBehaviour
{
    // Camera follow script
    public CameraFollow cameraFollow;

    // All invisible platforms in this level
    public InvisiblePlatforms[] hiddenPlatforms;

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player touches the reveal platform
        if (collision.gameObject.CompareTag("Player"))
        {
            // Zoom out the camera
            if (cameraFollow != null)
                cameraFollow.showingOverview = true;

            // Make all invisible platforms visible
            foreach (InvisiblePlatforms platform in hiddenPlatforms)
            {
                if (platform != null)
                    platform.SetOverview(true);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the player leaves the reveal platform
        if (collision.gameObject.CompareTag("Player"))
        {
            // Return the camera to normal
            if (cameraFollow != null)
                cameraFollow.showingOverview = false;

            // Hide all platforms again
            foreach (InvisiblePlatforms platform in hiddenPlatforms)
            {
                if (platform != null)
                    platform.SetOverview(false);
            }
        }
    }
}
