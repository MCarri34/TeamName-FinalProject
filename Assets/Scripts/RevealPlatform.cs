using UnityEngine;
using UnityEngine.Serialization;

public class RevealPlatform : MonoBehaviour
{
    // Camera follow script
    public CameraFollow cameraFollow;

    // All invisible platforms in this level
    public InvisiblePlatforms[] hiddenPlatforms;

    [Header("Audio")]
    public AudioSource audioSource;
    [FormerlySerializedAs("revealSound")]
    public AudioClip enterSound;
    public AudioClip exitSound;
    [FormerlySerializedAs("revealVolume")]
    [Range(0f, 1f)] public float enterVolume = 1f;
    [Range(0f, 1f)] public float exitVolume = 1f;

    private bool isRevealing;

    // Items can call this directly to reveal the level and play its sound.
    public void BeginReveal()
    {
        if (isRevealing) return;
        isRevealing = true;

        if (cameraFollow != null)
            cameraFollow.showingOverview = true;

        foreach (InvisiblePlatforms platform in hiddenPlatforms)
        {
            if (platform != null)
                platform.SetOverview(true);
        }

        if (audioSource != null && enterSound != null)
            audioSource.PlayOneShot(enterSound, enterVolume);
    }

    public void EndReveal()
    {
        if (!isRevealing) return;
        isRevealing = false;

        if (cameraFollow != null)
            cameraFollow.showingOverview = false;

        foreach (InvisiblePlatforms platform in hiddenPlatforms)
        {
            if (platform != null)
                platform.SetOverview(false);
        }

        if (audioSource != null && exitSound != null)
            audioSource.PlayOneShot(exitSound, exitVolume);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player touches the reveal platform
        if (collision.gameObject.CompareTag("Player"))
        {
            BeginReveal();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the player leaves the reveal platform
        if (collision.gameObject.CompareTag("Player"))
        {
            EndReveal();
        }
    }
}
