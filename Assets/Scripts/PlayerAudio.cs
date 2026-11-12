using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{

    [Header("Audio")]
    public AudioClip[] footstepClips;
    public AudioClip jumpStartClip;
    public AudioClip landClip;
    [SerializeField] private AudioSource SoundSource;
    //public AudioClip dashClip;

    [Range(0f, 1f)] public float walkingVolume = 0.5f;
    [Range(0f, 1f)] public float jumpVolume = 0.7f;
    [Range(0f, 2f)] public float landVolume = 1.0f;
    //[Range(0f, 1f)] public float dashVolume = 0.8f;
    private static readonly int[] ScaleSemitones =
    {
        0, 2, 4, 5, 7, 9, 11, 12
    };

    private readonly Dictionary<GameObject, int> platformNotes = new();

    public void PlayLanding(GameObject platform)
    {
        if (platform == null || SoundSource == null || landClip == null)
        {
            return;
        }

        if (!platformNotes.TryGetValue(platform, out int noteIndex))
        {
            noteIndex = Mathf.Min(platformNotes.Count, ScaleSemitones.Length - 1);
            platformNotes.Add(platform, noteIndex);
        }

        int semitones = ScaleSemitones[noteIndex];
        SoundSource.pitch = Mathf.Pow(2f, semitones / 12f);
        SoundSource.PlayOneShot(landClip, landVolume);
    }
}
