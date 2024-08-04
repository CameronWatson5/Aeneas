using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public Button mainMenuButton;
    public Button exitButton;

    void Start()
    {
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        }
        else
        {
            Debug.LogError("MainMenuButton is not assigned in the Inspector.");
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitGame);
        }
        else
        {
            Debug.LogError("ExitButton is not assigned in the Inspector.");
        }
    }

    void GoToMainMenu()
    {
        if (MissionManager.Instance != null)
        {
            MissionManager.Instance.ResetMissionIndex();
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }

        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.TransitionToScene("MainMenu", "");
        }
        else
        {
            Debug.LogError("GameOverManager: SceneTransitionManager not found");
        }
    }

    void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }
}