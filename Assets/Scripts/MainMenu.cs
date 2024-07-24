using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.StartNewGame("Troy", "SpawnPoint1");
        }
        else
        {
            Debug.LogError("SceneTransitionManager instance is not set.");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}