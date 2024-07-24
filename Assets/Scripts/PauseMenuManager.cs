using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public Button resumeButton;
    public Button quitButton;
    public Canvas pauseMenuCanvas;
    private string previousScene;

    void Start()
    {
        // Retrieve the previous scene name from PlayerPrefs
        previousScene = PlayerPrefs.GetString("PreviousScene");

        // Ensure the pause menu canvas is active
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("PauseMenuCanvas is not assigned in the Inspector.");
        }

        // Assign button listeners
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(ResumeGame);
        }
        else
        {
            Debug.LogError("ResumeButton is not assigned in the Inspector.");
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
        else
        {
            Debug.LogError("QuitButton is not assigned in the Inspector.");
        }
    }

    void Update()
    {
        // Handle Escape key to resume game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(previousScene))
        {
            SceneManager.UnloadSceneAsync("PauseMenu");
        }
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}