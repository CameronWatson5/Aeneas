// The special popups are different from regular popups.
// This is because they look different, as they use a black prefab, and also, they
// transition the player to a new scene.

using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpecialPopUp : MonoBehaviour
{
    public GameObject popupPanel;
    public TMP_Text instructionsText;
    public Button closeButton;
    [Range(0f, 1f)]
    public float typewriterVolume = 1f;

    public static bool IsSpecialPopupActive { get; private set; }

    public event Action OnPopupClosed;

    private AudioSource typewriterAudioSource;
    private AudioClip typewriterSound;
    private Coroutine typingCoroutine;

    private void Awake()
    {
        InitializeComponents();
        closeButton.onClick.AddListener(ClosePopup);
    }

    public void InitializeComponents()
    {
        if (popupPanel == null || instructionsText == null || closeButton == null)
        {
            Debug.LogError("SpecialPopUp: One or more UI components are not assigned.");
            return;
        }

        if (typewriterAudioSource == null)
        {
            typewriterAudioSource = gameObject.AddComponent<AudioSource>();
        }
        typewriterAudioSource.volume = typewriterVolume;
        typewriterAudioSource.playOnAwake = false;
        typewriterAudioSource.loop = true;

        SetTypewriterSound();

        popupPanel.SetActive(false);
    }

    private void SetTypewriterSound()
    {
        if (typewriterSound == null)
        {
            typewriterSound = Resources.Load<AudioClip>("Sound Effects/468927__malakme__medium-text-blip(1)");
            if (typewriterSound == null)
            {
                Debug.LogError("Failed to load typewriter sound effect.");
            }
        }
    }

    public void ShowPopup(string text)
    {
        if (popupPanel == null || instructionsText == null || closeButton == null)
        {
            Debug.LogError("SpecialPopUp: Cannot show popup because one or more UI components are not assigned.");
            return;
        }

        instructionsText.text = "";
        instructionsText.fontSize = 14;

        popupPanel.SetActive(true);
        IsSpecialPopupActive = true;

        // Make the popup cover the entire screen
        RectTransform rectTransform = popupPanel.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        // Center the text
        RectTransform textRectTransform = instructionsText.GetComponent<RectTransform>();
        textRectTransform.anchorMin = new Vector2(0.1f, 0.1f);
        textRectTransform.anchorMax = new Vector2(0.9f, 0.9f);
        textRectTransform.anchoredPosition = Vector2.zero;
        textRectTransform.sizeDelta = Vector2.zero;

        // Position the close button at the top right
        RectTransform buttonRectTransform = closeButton.GetComponent<RectTransform>();
        buttonRectTransform.anchorMin = new Vector2(1, 1);
        buttonRectTransform.anchorMax = new Vector2(1, 1);
        buttonRectTransform.pivot = new Vector2(1, 1);
        buttonRectTransform.anchoredPosition = new Vector2(-2, -2); // Offset from top right corner
        buttonRectTransform.sizeDelta = new Vector2(20, 20);

        Time.timeScale = 0; // Pause the game

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText(text));
    }

    private IEnumerator TypeText(string text)
    {
        instructionsText.text = "";
        if (typewriterSound != null)
        {
            typewriterAudioSource.clip = typewriterSound;
            typewriterAudioSource.Play();
        }

        foreach (char letter in text.ToCharArray())
        {
            instructionsText.text += letter;
            yield return new WaitForSecondsRealtime(0.05f); // Adjust the typing speed here
        }

        StopTypewriterSound();
    }

    public void ClosePopup()
    {
        if (popupPanel != null)
        {
            Debug.Log("Closing popup.");
            popupPanel.SetActive(false);
            Time.timeScale = 1; // Resume the game
            IsSpecialPopupActive = false;

            StopTypewriterSound();

            OnPopupClosed?.Invoke();
        }
    }

    private void StopTypewriterSound()
    {
        if (typewriterAudioSource != null && typewriterAudioSource.isPlaying)
        {
            typewriterAudioSource.Stop();
        }

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
    }

    public void SetTypewriterVolume(float volume)
    {
        typewriterVolume = Mathf.Clamp01(volume);
        if (typewriterAudioSource != null)
        {
            typewriterAudioSource.volume = typewriterVolume;
        }
    }
}
