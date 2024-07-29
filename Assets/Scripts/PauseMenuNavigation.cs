using UnityEngine;
using UnityEngine.UI;

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

    public Button mapButton;
    public Button inventoryButton;
    public Button controlsButton;
    public Button helpButton;
    public Button logButton;
    public Button missionButton;

    void Start()
    {
        mapButton.onClick.AddListener(() => SwitchPanel(mapPanel));
        inventoryButton.onClick.AddListener(() => SwitchPanel(inventoryPanel));
        controlsButton.onClick.AddListener(() => SwitchPanel(controlsPanel));
        helpButton.onClick.AddListener(() => SwitchPanel(helpPanel));
        logButton.onClick.AddListener(() => SwitchPanel(logPanel));
        missionButton.onClick.AddListener(() => SwitchPanel(missionPanel));
        
        // Show the first panel by default
        SwitchPanel(inventoryPanel);
    }

    void SwitchPanel(GameObject panelToShow)
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
    }
}