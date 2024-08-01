using UnityEngine;
using UnityEngine.UI;

public class FlashingIcon : MonoBehaviour
{
    public float flashDuration = 1f;
    private Image image; 
    private bool isVisible = true;
    private float nextToggleTime;

    void Start()
    {
        image = GetComponent<Image>(); 
        if (image == null)
        {
            Debug.LogError("Image component not found on the FlashingIcon object.");
        }
        nextToggleTime = Time.unscaledTime + flashDuration;
    }

    void Update()
    {
        if (Time.unscaledTime >= nextToggleTime)
        {
            ToggleVisibility();
            nextToggleTime = Time.unscaledTime + flashDuration;
        }
    }

    void ToggleVisibility()
    {
        isVisible = !isVisible;
        image.enabled = isVisible;
    }
}