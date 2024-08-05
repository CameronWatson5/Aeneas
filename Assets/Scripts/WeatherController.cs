using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public ParticleSystem rainSystem;
    public Light moonLight;
    public AudioSource rainSound;

    [Range(0f, 1f)]
    public float rainIntensity = 1f;

    void Update()
    {
        // Adjust rain particle emission
        var emission = rainSystem.emission;
        emission.rateOverTime = 2000f * rainIntensity;

        // Adjust moon light intensity
        moonLight.intensity = 0.2f + (0.3f * (1f - rainIntensity));

        // Adjust rain sound volume
        rainSound.volume = rainIntensity;
    }
}