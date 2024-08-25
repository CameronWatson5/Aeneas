// This script is related to the weather, including the rain, rain sound effect, and nighttime effect
// in the TroySack scene

using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class WeatherController2D : MonoBehaviour
{
    public ParticleSystem rainSystem;
    public Light2D moonLight;
    public AudioSource rainSound;
    public Image nightOverlay;

    [Range(0f, 1f)]
    public float rainIntensity = 1f;

    [Range(0f, 1f)]
    public float nightIntensity = 0.5f;

    void Start()
    {
        Debug.Log("WeatherController2D started");
        if (rainSystem == null)
        {
            Debug.LogError("Rain System is not assigned!");
        }
        else
        {
            Debug.Log($"Rain System assigned: {rainSystem.name}");
            Debug.Log($"Initial emission rate: {rainSystem.emission.rateOverTime.constant}");
        }
    }

    void Update()
    {
        if (rainSystem != null)
        {
            var emission = rainSystem.emission;
            float newRate = 500f * rainIntensity;
            emission.rateOverTime = newRate;
            Debug.Log($"Updated emission rate: {newRate}");
        }

        if (moonLight != null)
        {
            moonLight.intensity = 0.2f + (0.3f * (1f - nightIntensity));
            Debug.Log($"Moon light intensity: {moonLight.intensity}");
        }

        if (nightOverlay != null)
        {
            Color overlayColor = nightOverlay.color;
            overlayColor.a = 0.5f * nightIntensity;
            nightOverlay.color = overlayColor;
            Debug.Log($"Night overlay alpha: {overlayColor.a}");
        }

        if (rainSound != null)
        {
            rainSound.volume = rainIntensity;
            Debug.Log($"Rain sound volume: {rainSound.volume}");
        }
    }
}