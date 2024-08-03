using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpecialPopUp : MonoBehaviour
{
    public GameObject popupPanel;
    public TMP_Text instructionsText;
    public Button closeButton;

    public event Action OnPopupClosed;

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

        popupPanel.SetActive(false);
    }

    public void ShowPopup(string text)
    {
        if (popupPanel == null || instructionsText == null || closeButton == null)
        {
            Debug.LogError("SpecialPopUp: Cannot show popup because one or more UI components are not assigned.");
            return;
        }

        instructionsText.text = text;
        instructionsText.fontSize = 14; // Adjust font size
        popupPanel.SetActive(true);

        RectTransform rectTransform = popupPanel.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = new Vector2(300, 150); // Adjust size to fit within the screen

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
        buttonRectTransform.anchoredPosition = new Vector2(-10, -10); // Offset from top right corner
        buttonRectTransform.sizeDelta = new Vector2(30, 30);

        Time.timeScale = 0; // Pause the game
    }

    public void ClosePopup()
    {
        if (popupPanel != null)
        {
            Debug.Log("Closing popup.");
            popupPanel.SetActive(false);
            Time.timeScale = 1; // Resume the game

            OnPopupClosed?.Invoke();
        }
    }
}
