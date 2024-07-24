using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUp : MonoBehaviour
{
    public GameObject popupPanel;
    public Button closeButton;
    public TextMeshProUGUI instructionsText;
    public float delayBeforePopup = 1f; // Delay before showing the popup

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
            StartCoroutine(ShowPopupWithDelay());
        }
        else
        {
            popupPanel.SetActive(false);
        }
    }

    IEnumerator ShowPopupWithDelay()
    {
        Debug.Log("Starting delay before showing popup");
        yield return new WaitForSecondsRealtime(delayBeforePopup);
        ShowPopup();
    }

    void ShowPopup()
    {
        if (popupPanel != null && !popupPanel.activeSelf) // Ensure the popup is not already active
        {
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
        }
        else
        {
            Debug.LogError("Popup panel is missing or not active.");
        }
    }

    public void SetPopupText(string text)
    {
        if (instructionsText != null)
        {
            instructionsText.text = text;
        }
        else
        {
            Debug.LogError("Instructions text is not assigned.");
        }
    }

    public void ResetPopup()
    {
        Debug.Log("Resetting popup for new session");
        hasShownThisSession = false;
    }
}
