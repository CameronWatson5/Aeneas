// This script is used for the rain effect in the TroySack scene.

using UnityEngine;

public class RainController : MonoBehaviour
{
    public ParticleSystem rainSystem;
    public float baseWindStrength = 1f;
    public float windVariation = 0.5f;
    public float windChangeSpeed = 0.1f;

    private ParticleSystem.VelocityOverLifetimeModule velocityModule;
    private float currentWindStrength;

    void Start()
    {
        if (rainSystem != null)
        {
            velocityModule = rainSystem.velocityOverLifetime;
            velocityModule.enabled = true;
        }
    }

    void Update()
    {
        UpdateWind();
    }

    void UpdateWind()
    {
        currentWindStrength = baseWindStrength + Mathf.Sin(Time.time * windChangeSpeed) * windVariation;
        
        if (velocityModule.enabled)
        {
            velocityModule.x = currentWindStrength;
        }
    }
}