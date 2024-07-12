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

    // Static variable to track if the popup has been shown this game session
    private static bool hasShownThisSession = false;

    void Start()
    {
        closeButton.onClick.AddListener(ClosePopup);

        if (!hasShownThisSession)
        {
            ShowPopup();
        }
        else
        {
            popupPanel.SetActive(false);
        }
    }

    void ShowPopup()
    {
        popupPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void ClosePopup()
    {
        popupPanel.SetActive(false);
        Time.timeScale = 1f;
        
        // Mark that popup has shown this session
        hasShownThisSession = true;
    }

    public void SetPopupText(string text)
    {
        if (instructionsText != null)
        {
            instructionsText.text = text;
        }
    }

    public void ResetPopup()
    {
        hasShownThisSession = false;
    }
}