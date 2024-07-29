using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUp : MonoBehaviour
{
    public GameObject popupPanel;
    public Button closeButton;
    public TextMeshProUGUI instructionsText;
    public float defaultDelayBeforePopup = 1f; // Default delay before showing the popup

    private static Dictionary<string, bool> shownPopups = new Dictionary<string, bool>();

    void Start()
    {
        if (closeButton == null || popupPanel == null || instructionsText == null)
        {
            Debug.LogError("PopUp UI elements are not assigned properly in the Inspector.");
            return;
        }

        closeButton.onClick.AddListener(ClosePopup);
        popupPanel.SetActive(false);
    }

    public IEnumerator ShowPopupWithDelay(float delay, string text, string popupId)
    {
        if (!HasShownPopup(popupId))
        {
            Debug.Log("Starting delay before showing popup");
            yield return new WaitForSecondsRealtime(delay);
            ShowPopup(text, popupId);
        }
    }

    public void ShowPopup(string text, string popupId)
    {
        if (!HasShownPopup(popupId))
        {
            if (popupPanel != null && !popupPanel.activeSelf) // Ensure the popup is not already active
            {
                instructionsText.text = text;
                Debug.Log("Showing popup");
                popupPanel.SetActive(true);
                Time.timeScale = 0f;
                MarkPopupAsShown(popupId);
            }
            else
            {
                Debug.LogError("Popup panel is missing or already active.");
            }
        }
    }

    private void ClosePopup()
    {
        if (popupPanel != null && popupPanel.activeSelf) // Ensure the popup is currently active
        {
            Debug.Log("Closing popup");
            popupPanel.SetActive(false);
            Time.timeScale = 1f;

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

    private bool HasShownPopup(string popupId)
    {
        return shownPopups.ContainsKey(popupId) && shownPopups[popupId];
    }

    private void MarkPopupAsShown(string popupId)
    {
        if (!shownPopups.ContainsKey(popupId))
        {
            shownPopups.Add(popupId, true);
        }
    }

    public void ResetPopup()
    {
        shownPopups.Clear();
        Debug.Log("Resetting popup for new session");
    }

    public void ShowPopupOnSceneLoad(string popupId, string popupText, float popupDelay)
    {
        StartCoroutine(ShowPopupWithDelay(popupDelay, popupText, popupId));
    }
}
