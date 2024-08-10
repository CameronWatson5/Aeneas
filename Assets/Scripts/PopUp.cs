using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    public GameObject popupPanel;     // The panel containing the pop-up UI
    public TMP_Text instructionsText; // The text component to display the message
    public Button closeButton;        // The button to close the pop-up

    public static bool IsPopupActive { get; private set; } // Tracks if a pop-up is active

    private void Awake()
    {
        // Initialize UI components and set up the close button listener
        InitializeComponents();
        closeButton.onClick.AddListener(ClosePopup);
    }

    public void InitializeComponents()
    {
        // Ensure that all UI components are assigned
        if (popupPanel == null || instructionsText == null || closeButton == null)
        {
            Debug.LogError("PopUp: One or more UI components are not assigned.");
            return;
        }

        // Initially hide the pop-up panel
        popupPanel.SetActive(false);
    }

    public void ShowPopup(string text, string popupId)
    {
        // Ensure that the pop-up can be shown
        if (popupPanel == null || instructionsText == null || closeButton == null)
        {
            Debug.LogError("PopUp: Cannot show popup because one or more UI components are not assigned.");
            return;
        }

        // Set up the pop-up text and make the panel visible
        instructionsText.text = text;
        instructionsText.fontSize = 14;
        popupPanel.SetActive(true);
        IsPopupActive = true;

        // Adjust the UI layout and positioning
        RectTransform rectTransform = popupPanel.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = new Vector2(300, 150);

        // Center the text
        RectTransform textRectTransform = instructionsText.GetComponent<RectTransform>();
        textRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        textRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        textRectTransform.pivot = new Vector2(0.5f, 0.5f);
        textRectTransform.anchoredPosition = Vector2.zero;
        textRectTransform.sizeDelta = new Vector2(280, 70);

        // Position the close button at the top right
        RectTransform buttonRectTransform = closeButton.GetComponent<RectTransform>();
        buttonRectTransform.anchorMin = new Vector2(1, 1);
        buttonRectTransform.anchorMax = new Vector2(1, 1);
        buttonRectTransform.pivot = new Vector2(1, 1);
        buttonRectTransform.anchoredPosition = new Vector2(-10, -10);
        buttonRectTransform.sizeDelta = new Vector2(30, 30);

        // Pause the game while the pop-up is active
        Time.timeScale = 0;
    }

    public void ClosePopup()
    {
        // Close the pop-up and resume the game
        if (popupPanel != null)
        {
            Debug.Log("Closing popup.");
            popupPanel.SetActive(false);
            Time.timeScale = 1;
            IsPopupActive = false;
        }
    }

    public void ShowPopupOnSceneLoad(string popupId, string popupText, float popupDelay)
    {
        // Show a pop-up with a delay when a scene loads
        StartCoroutine(ShowPopupWithDelay(popupDelay, popupText, popupId));
    }

    public IEnumerator ShowPopupWithDelay(float delay, string text, string popupId)
    {
        yield return new WaitForSecondsRealtime(delay);
        ShowPopup(text, popupId);
    }
}
