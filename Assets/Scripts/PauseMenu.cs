using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    private static bool GameIsPaused = false;
    private string currentSceneName;
    private MapManager mapManager;

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

        // Only unload if the scene is actually loaded
        if (SceneManager.GetSceneByName("PauseMenu").isLoaded)
        {
            SceneManager.UnloadSceneAsync("PauseMenu");
        }

        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.FindPlayer();
            cameraFollow.CalculateBounds();
            cameraFollow.isNewScene = true; // Force camera to center on player
        }
    }

    void Pause()
    {
        Debug.Log("Pausing game to PauseMenu");
        GameIsPaused = true;
        Time.timeScale = 0f;
        // Save the current scene name to PlayerPrefs
        PlayerPrefs.SetString("PreviousScene", currentSceneName);
        StartCoroutine(LoadPauseMenuScene());
    }

    IEnumerator LoadPauseMenuScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("PauseMenu", LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        InitializeMap();
    }

    void InitializeMap()
    {
        mapManager = FindObjectOfType<MapManager>();
        if (mapManager != null)
        {
            mapManager.SetupMapForScene(currentSceneName);
        }
        else
        {
            Debug.LogError("MapManager not found in the scene.");
        }
    }
}
