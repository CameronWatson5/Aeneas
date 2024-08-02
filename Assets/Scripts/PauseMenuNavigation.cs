using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PauseMenuNavigation : MonoBehaviour
{
    public GameObject backgroundPanel;
    public GameObject sideNavigationBar;
    public GameObject mapPanel;
    public GameObject inventoryPanel;
    public GameObject controlsPanel;
    public GameObject helpPanel;
    public GameObject logPanel;
    public GameObject missionPanel;
    public GameObject savePanel;
    public GameObject loadPanel;

    public Button mapButton;
    public Button inventoryButton;
    public Button controlsButton;
    public Button helpButton;
    public Button logButton;
    public Button missionButton;
    public Button resumeButton; 
    public Button quitButton;
    //public Button saveButton;
    //public Button loadButton;

    public Color normalColor = Color.white;
    public Color pressedColor = Color.gray;
    public Color highlightedColor = Color.gray; // Added highlight color

    private Dictionary<Button, GameObject> buttonPanelPairs;
    private Button activeButton;

    void Start()
    {
        buttonPanelPairs = new Dictionary<Button, GameObject>
        {
            { mapButton, mapPanel },
            { inventoryButton, inventoryPanel },
            { controlsButton, controlsPanel },
            { helpButton, helpPanel },
            { logButton, logPanel },
            { missionButton, missionPanel },
            //{ saveButton, savePanel },
            //{ loadButton, loadPanel }
        };

        foreach (var pair in buttonPanelPairs)
        {
            pair.Key.onClick.AddListener(() => SwitchPanel(pair.Value, pair.Key));
        }

        // Show the inventory panel by default and hide the inventory button
        SwitchPanel(inventoryPanel, inventoryButton);
    }

    void SwitchPanel(GameObject panelToShow, Button buttonToRemove)
    {
        // Ensure the side navigation bar remains active
        if (sideNavigationBar != null)
        {
            sideNavigationBar.SetActive(true);
        }

        // Hide all panels except the side navigation bar
        foreach (Transform child in backgroundPanel.transform)
        {
            if (child.gameObject != sideNavigationBar)
            {
                child.gameObject.SetActive(false);
            }
        }

        // Show the selected panel
        panelToShow.SetActive(true);

        // Hide the selected button and show all others
        foreach (var pair in buttonPanelPairs)
        {
            pair.Key.gameObject.SetActive(pair.Key != buttonToRemove);
        }

        // Update the colors of the buttons
        if (activeButton != null)
        {
            SetButtonColors(activeButton, normalColor);
        }

        SetButtonColors(buttonToRemove, pressedColor);
        activeButton = buttonToRemove;

        // Make sure resume and quit buttons match the normal color and highlighted color
        SetButtonColors(resumeButton, normalColor);
        SetButtonColors(quitButton, normalColor);
    }

    void SetButtonColors(Button button, Color color)
    {
        ColorBlock colorBlock = button.colors;
        colorBlock.normalColor = color;
        colorBlock.highlightedColor = highlightedColor; // Set the highlighted color
        colorBlock.pressedColor = pressedColor;
        button.colors = colorBlock;
    }
}
