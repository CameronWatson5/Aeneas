using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class UIAdjuster : MonoBehaviour
{
    public RectTransform popupPanel;
    public RectTransform closeButton;
    public TextMeshProUGUI buttonText; 

    void Start()
    {
        AdjustPopupSize();
        PositionCloseButton();
        AdjustButtonTextSize();
    }

    void AdjustPopupSize()
    {
        if (popupPanel != null)
        {
            popupPanel.sizeDelta = new Vector2(Screen.width * 0.6f, Screen.height * 0.4f);
        }
    }

    void PositionCloseButton()
    {
        if (closeButton != null)
        {
            closeButton.anchorMin = new Vector2(1, 1);
            closeButton.anchorMax = new Vector2(1, 1);
            closeButton.anchoredPosition = new Vector2(-10, -10);
            closeButton.sizeDelta = new Vector2(50, 30);
        }
    }

    void AdjustButtonTextSize()
    {
        if (buttonText != null)
        {
            buttonText.fontSize = 14; 
        }
    }
}
