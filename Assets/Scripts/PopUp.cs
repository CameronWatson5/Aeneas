using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUp : MonoBehaviour
{
    public GameObject popupPanel;
    public Button closeButton;
    public TextMeshProUGUI instructionsText;
    public float defaultDelayBeforePopup = 1f; // Default delay before showing the popup

    // Static variable to track if the popup has been shown this game session
    private static bool hasShownThisSession = false;

    void Start()
    {
        if (closeButton == null || popupPanel == null || instructionsText == null)
        {
            Debug.LogError("PopUp UI elements are not assigned properly in the Inspector.");
            return;
        }

        closeButton.onClick.AddListener(ClosePopup);

        if (!hasShownThisSession)
        {
            StartCoroutine(ShowPopupWithDelay(defaultDelayBeforePopup, instructionsText.text));
        }
        else
        {
            popupPanel.SetActive(false);
        }
    }

    public IEnumerator ShowPopupWithDelay(float delay, string text)
    {
        Debug.Log("Starting delay before showing popup");
        yield return new WaitForSecondsRealtime(delay);
        ShowPopup(text);
    }

    public void ShowPopup(string text)
    {
        if (popupPanel != null && !popupPanel.activeSelf) // Ensure the popup is not already active
        {
            instructionsText.text = text;
            Debug.Log("Showing popup");
            popupPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            Debug.LogError("Popup panel is missing or already active.");
        }
    }

    void ClosePopup()
    {
        if (popupPanel != null && popupPanel.activeSelf) // Ensure the popup is currently active
        {
            Debug.Log("Closing popup");
            popupPanel.SetActive(false);
            Time.timeScale = 1f;

            // Mark that popup has shown this session
            hasShownThisSession = true;

            // Add the popup text to the log
            if (LogManager.Instance != null)
            {
                LogManager.Instance.AddLogEntry(instructionsText.text);
            }
            else
            {
                Debug.LogError("LogManager instance not found.");
            }
        }
        else
        {
            Debug.LogError("Popup panel is missing or not active.");
        }
    }

    public void ResetPopup()
    {
        Debug.Log("Resetting popup for new session");
        hasShownThisSession = false;
    }
}
