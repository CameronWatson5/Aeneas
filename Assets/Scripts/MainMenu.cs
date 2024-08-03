using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }

    public Button startButton;
    public Button quitButton;
    public GameObject mainMenuElements;
    public GameObject mainMenuCutscenePanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log("MainMenu: Start called");
        InitializeMissionManager();
        SetupButtons();
        SetupMainMenuCutscenePanel();
    }

    private void InitializeMissionManager()
    {
        if (MissionManager.Instance == null)
        {
            GameObject missionManager = new GameObject("MissionManager");
            missionManager.AddComponent<MissionManager>();
            DontDestroyOnLoad(missionManager);
        }
        else
        {
            MissionManager.Instance.ResetMissionIndex(); // Ensure the mission index is reset
        }
    }

    private void SetupButtons()
    {
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
    }

    private void SetupMainMenuCutscenePanel()
    {
        if (mainMenuCutscenePanel != null)
        {
            mainMenuCutscenePanel.SetActive(false);
            Debug.Log("MainMenu: Main menu cutscene panel set to inactive");
        }
        else Debug.LogError("MainMenu: MainMenuCutscenePanel reference is missing");
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

        if (mainMenuCutscenePanel != null)
        {
            mainMenuCutscenePanel.SetActive(true);
            Debug.Log("MainMenu: Main menu cutscene panel activated");

            MainMenuCutsceneManager cutsceneManager = mainMenuCutscenePanel.GetComponent<MainMenuCutsceneManager>();
            if (cutsceneManager != null)
            {
                cutsceneManager.enabled = true;
                Debug.Log("MainMenu: MainMenuCutsceneManager enabled");
            }
            else Debug.LogError("MainMenu: MainMenuCutsceneManager component is missing on the CutscenePanel");
        }
        else Debug.LogError("MainMenu: MainMenuCutscenePanel reference is missing");
    }

    public void QuitGame()
    {
        Debug.Log("MainMenu: QuitGame called");
        Application.Quit();
    }
}
