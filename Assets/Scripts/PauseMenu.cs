// This script is used to transition the game from gameplay to the pause menu

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }
    public static bool GameIsPaused = false;
    private string currentSceneName;
    private MapManager mapManager;

    // List of scenes where pausing is not allowed
    private readonly List<string> nonPausableScenes = new List<string> { "MainMenu", "GameOver" };

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

    void Start()
    {
        UpdateCurrentSceneName();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateCurrentSceneName();
    }

    void UpdateCurrentSceneName()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log("Current Scene: " + currentSceneName);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !nonPausableScenes.Contains(currentSceneName))
        {
            Debug.Log("Q key pressed. GameIsPaused: " + GameIsPaused);
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

        if (SceneManager.GetSceneByName("PauseMenu").isLoaded)
        {
            SceneManager.UnloadSceneAsync("PauseMenu").completed += OnPauseMenuUnloaded;
        }
    }

    private void OnPauseMenuUnloaded(AsyncOperation obj)
    {
        Debug.Log("PauseMenu scene unloaded.");
        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.FindPlayer();
            cameraFollow.CalculateBounds();
            cameraFollow.isNewScene = true;
        }
    }

    public void Pause()
    {
        if (nonPausableScenes.Contains(currentSceneName))
        {
            Debug.Log("Cannot pause in " + currentSceneName);
            return;
        }

        Debug.Log("Pausing game to PauseMenu");
        GameIsPaused = true;
        Time.timeScale = 0f;
        PlayerPrefs.SetString("PreviousScene", currentSceneName);
        if (!SceneManager.GetSceneByName("PauseMenu").isLoaded)
        {
            StartCoroutine(LoadPauseMenuScene());
        }
        else
        {
            Debug.Log("PauseMenu scene is already loaded.");
        }
    }

    IEnumerator LoadPauseMenuScene()
    {
        Debug.Log("Loading PauseMenu scene...");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("PauseMenu", LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        InitializeMap();
        Debug.Log("PauseMenu scene loaded.");
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

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}