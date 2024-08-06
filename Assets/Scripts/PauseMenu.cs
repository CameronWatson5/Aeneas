using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }
    public static bool GameIsPaused = false;
    private string currentSceneName;
    private MapManager mapManager;

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
        currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log("Current Scene: " + currentSceneName);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key pressed. GameIsPaused: " + GameIsPaused);
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
        Debug.Log("Pausing game to PauseMenu");
        GameIsPaused = true;
        Time.timeScale = 0f;
        PlayerPrefs.SetString("PreviousScene", currentSceneName);
        StartCoroutine(LoadPauseMenuScene());
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
}
