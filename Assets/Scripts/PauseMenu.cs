using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private static bool GameIsPaused = false;
    private string currentSceneName;

    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log("Current Scene: " + currentSceneName);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Debug.Log("Resuming game from PauseMenu");
        GameIsPaused = false;
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync("PauseMenu");
    }

    void Pause()
    {
        Debug.Log("Pausing game to PauseMenu");
        GameIsPaused = true;
        Time.timeScale = 0f;
        // Save the current scene name to PlayerPrefs
        PlayerPrefs.SetString("PreviousScene", currentSceneName);
        SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
    }
}