// This script is played in the main menu when the start game button is pressed.
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
    private Button button;
    private AudioSource audioSource;

    void Awake()
    {
        button = GetComponent<Button>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Ensure Play On Awake is disabled
        audioSource.playOnAwake = false;

        button.onClick.AddListener(PlaySound);
    }

    void PlaySound()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
            Debug.Log("Button click sound played.");
        }
        else
        {
            Debug.LogWarning("AudioSource does not have an AudioClip assigned.");
        }
    }
}