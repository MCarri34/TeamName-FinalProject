
using UnityEngine;

public class GlassBreakEvent : MonoBehaviour
{
    [Header("Glass Particle System")]
    public ParticleSystem glassShards;

    [Header("Trail")]
    public ParticleSystem fallingTrail;

    [Header("Audio")]
    public AudioSource sfxSource;
    public AudioClip glassBreakSound;


    public AudioSource windSource;
    public AudioClip fallingWindSound;

    [Range(0f, 1f)] public float glassVolume = 1f;
    [Range(0f, 1f)] public float windVolume = 1f;

    [Header("Simulation Speeds")]
    public float burstSpeed = 1.8f;
    public float slowSpeed = 0.15f;
    public float normalSpeed = 1.0f;

    public void BreakGlass()
    {
        if (sfxSource != null && glassBreakSound != null)
            sfxSource.PlayOneShot(glassBreakSound, glassVolume);

        if (glassShards == null) return;

        glassShards.Stop(
            true,
            ParticleSystemStopBehavior.StopEmittingAndClear
        );

        SetSpeed(burstSpeed);
        glassShards.Play();
    }

    public void SlowGlass()
    {
        SetSpeed(slowSpeed);
    }

    public void ResumeGlass()
    {
        SetSpeed(normalSpeed);
    }

    public void StartFallingTrail()
    {
        if (fallingTrail != null && !fallingTrail.isPlaying)
            fallingTrail.Play();

        if (windSource == null || fallingWindSound == null)
            return;

        if (windSource.isPlaying) return;

        windSource.clip = fallingWindSound;
        windSource.volume = windVolume;
        windSource.loop = true;
        windSource.Play();
    }

    public void StopFallingSound()
    {
        if (windSource != null)
            windSource.Stop();
    }

    private void OnDisable()
    {
        StopFallingSound();
    }

    private void SetSpeed(float speed)
    {
        if (glassShards == null) return;

        var main = glassShards.main;
        main.simulationSpeed = speed;
    }
}


