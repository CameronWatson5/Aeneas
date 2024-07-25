using UnityEngine;
using UnityEngine.SceneManagement;
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
        Time.timeScale = 1f; 
        ResetPlayerAttributes();
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.TransitionToScene("MainMenu", "DefaultSpawnPoint");
        }
        else
        {
            Debug.LogError("SceneTransitionManager instance is not set.");
        }
    }

    void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }

    void ResetPlayerAttributes()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            AeneasAttributes attributes = player.GetComponent<AeneasAttributes>();
            if (attributes != null)
            {
                attributes.ResetAttributes();
            }
            else
            {
                Debug.LogError("AeneasAttributes component not found on Player.");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found.");
        }
    }
}