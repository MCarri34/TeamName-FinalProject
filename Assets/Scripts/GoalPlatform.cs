using UnityEngine;

public class GoalPlatform : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip goalSound;
    [Range(0f, 1f)] public float goalVolume = 1f;

    private bool hasReached;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            ReachGoal();
    }

    public void ReachGoal()
    {
        if (hasReached) return;
        hasReached = true;

        if (audioSource != null && goalSound != null)
            audioSource.PlayOneShot(goalSound, goalVolume);
    }
}
