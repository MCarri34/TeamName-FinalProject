
using UnityEngine;

public class GlassBreakEvent : MonoBehaviour
{
    [Header("Glass Particle System")]
    public ParticleSystem glassShards;

    [Header("Trail")]
    public ParticleSystem fallingTrail;

    [Header("Simulation Speeds")]
    public float burstSpeed = 1.8f;
    public float slowSpeed = 0.15f;
    public float normalSpeed = 1.0f;

    // Glass breaks on impact
    public void BreakGlass()
    {
        if (glassShards == null) return;

        glassShards.Stop(
            true,
            ParticleSystemStopBehavior.StopEmittingAndClear
        );

        SetSpeed(burstSpeed);
        glassShards.Play();
    }

    // Slow motion during the pause
    public void SlowGlass()
    {
        SetSpeed(slowSpeed);
    }

    // Resume particle movement
    public void ResumeGlass()
    {
        SetSpeed(normalSpeed);
    }

    // Trail after character moving
    public void StartFallingTrail()
    {
        if (fallingTrail == null) return;

        if (!fallingTrail.isPlaying)
            fallingTrail.Play();
    }

    private void SetSpeed(float speed)
    {
        if (glassShards == null) return;

        var main = glassShards.main;
        main.simulationSpeed = speed;
    }
}


