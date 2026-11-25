using UnityEngine;
using UnityEngine.Serialization;

public class RevealPlatform : MonoBehaviour
{
    // Camera follow script
    public CameraFollow cameraFollow;
    [SerializeField] ParticleSystem revealParticleSystem;

    // Automatically find the invisible platforms
    private InvisiblePlatforms[] hiddenPlatforms;

    [Header("Audio")]
    public AudioSource audioSource;

    [FormerlySerializedAs("revealSound")]
    public AudioClip enterSound;
    public AudioClip exitSound;

    [FormerlySerializedAs("revealVolume")]
    [Range(0f, 1f)] public float enterVolume = 1f;
    [Range(0f, 1f)] public float exitVolume = 1f;

    private bool isRevealing = false;

    public void BeginReveal()
    {
        if (isRevealing)
            return;

        isRevealing = true;

        // Find all invisible platforms under this GameObject
        hiddenPlatforms =
            GetComponentsInChildren<InvisiblePlatforms>(true);

        // Tell the camera to zoom out
        if (cameraFollow != null)
        {
            cameraFollow.showingOverview = true;
        }

        // Reveal all invisible platforms
        foreach (InvisiblePlatforms platform in hiddenPlatforms)
        {
            platform.SetOverview(true);
        }
        revealParticleSystem.Play(true);

        // Show how many platforms were found
        Debug.Log("Found " + hiddenPlatforms.Length + " invisible platforms");

        // Play the reveal sound
        if (audioSource != null && enterSound != null)
        {
            audioSource.PlayOneShot(enterSound, enterVolume);
        }
    }

    public void EndReveal()
    {
        if (!isRevealing)
            return;

        isRevealing = false;

        // Tell the camera to zoom back in
        if (cameraFollow != null)
        {
            cameraFollow.showingOverview = false;
        }

        // Hide the platforms again
        if (hiddenPlatforms != null)
        {
            foreach (InvisiblePlatforms platform in hiddenPlatforms)
            {
                if (platform != null)
                    platform.SetOverview(false);
            }
        }
        revealParticleSystem.Play(false);
        revealParticleSystem.Stop();
        revealParticleSystem.Clear();

        // Play the exit sound
        if (audioSource != null && exitSound != null)
        {
            audioSource.PlayOneShot(exitSound, exitVolume);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Reveal the level when the player enters
        if (other.CompareTag("Player"))
        {
            BeginReveal();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Hide the level when the player leaves
        if (other.CompareTag("Player"))
        {
            EndReveal();
        }
    }
}
