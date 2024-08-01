using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;

    private void Start()
    {
        if (startButton != null) startButton.onClick.AddListener(StartGame);
        else Debug.LogError("StartButton reference is missing.");

        if (quitButton != null) quitButton.onClick.AddListener(QuitGame);
        else Debug.LogError("QuitButton reference is missing.");
    }

    public void StartGame()
    {
        Debug.Log("Starting new game.");
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
        Debug.Log("Quitting game.");
        Application.Quit();
    }
}