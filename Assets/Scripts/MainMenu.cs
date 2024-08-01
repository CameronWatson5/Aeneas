using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    public GameObject mainMenuElements; 
    public GameObject cutscenePanel;

    private void Start()
    {
        Debug.Log("MainMenu: Start called");
        
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
            Debug.Log("MainMenu: Start button listener added");
        }
        else Debug.LogError("MainMenu: StartButton reference is missing");

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
            Debug.Log("MainMenu: Quit button listener added");
        }
        else Debug.LogError("MainMenu: QuitButton reference is missing");

        if (cutscenePanel != null)
        {
            cutscenePanel.SetActive(false);
            Debug.Log("MainMenu: Cutscene panel set to inactive");
        }
        else Debug.LogError("MainMenu: CutscenePanel reference is missing");
    }

    public void StartGame()
    {
        Debug.Log("MainMenu: StartGame called");
        
        if (mainMenuElements != null)
        {
            mainMenuElements.SetActive(false);
            Debug.Log("MainMenu: Main menu elements deactivated");
        }
        else Debug.LogError("MainMenu: mainMenuElements reference is missing");

        if (cutscenePanel != null)
        {
            cutscenePanel.SetActive(true);
            Debug.Log("MainMenu: Cutscene panel activated");

            CutsceneManager cutsceneManager = cutscenePanel.GetComponent<CutsceneManager>();
            if (cutsceneManager != null)
            {
                cutsceneManager.enabled = true;
                Debug.Log("MainMenu: CutsceneManager enabled");
            }
            else Debug.LogError("MainMenu: CutsceneManager component is missing on the CutscenePanel");
        }
        else Debug.LogError("MainMenu: CutscenePanel reference is missing");
    }

    public void QuitGame()
    {
        Debug.Log("MainMenu: QuitGame called");
        Application.Quit();
    }
}