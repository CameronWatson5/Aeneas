using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [SerializeField] private float fadeDuration = 1f;
    private CanvasGroup fadeCanvasGroup;
    private string targetSceneName;
    private string spawnPointIdentifier;

    private void Awake()
    {
        Debug.Log("SceneTransitionManager: Awake called");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupFadeCanvas();
            InitializeManagers();
        }
        else
        {
            Debug.Log("SceneTransitionManager: Instance already exists, destroying duplicate");
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        MainMenuCutsceneManager cutsceneManager = FindObjectOfType<MainMenuCutsceneManager>();
        if (cutsceneManager != null)
        {
            cutsceneManager.OnCutsceneComplete += HandleCutsceneComplete;
        }
    }

    private void OnDisable()
    {
        MainMenuCutsceneManager cutsceneManager = FindObjectOfType<MainMenuCutsceneManager>();
        if (cutsceneManager != null)
        {
            cutsceneManager.OnCutsceneComplete -= HandleCutsceneComplete;
        }
    }

    private void InitializeManagers()
    {
        if (MissionManager.Instance == null)
        {
            Debug.Log("SceneTransitionManager: Creating MissionManager");
            GameObject missionManager = new GameObject("MissionManager");
            missionManager.AddComponent<MissionManager>();
            DontDestroyOnLoad(missionManager);
        }
    }

    private void SetupFadeCanvas()
    {
        Debug.Log("SceneTransitionManager: Setting up fade canvas");
        GameObject fadeCanvas = new GameObject("Fade Canvas");
        fadeCanvas.transform.SetParent(transform);
        Canvas canvas = fadeCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;

        CanvasScaler scaler = fadeCanvas.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        fadeCanvasGroup = fadeCanvas.AddComponent<CanvasGroup>();

        GameObject fadeImage = new GameObject("Fade Image");
        fadeImage.transform.SetParent(fadeCanvas.transform);
        RectTransform rectTransform = fadeImage.AddComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.anchoredPosition = Vector2.zero;

        Image image = fadeImage.AddComponent<Image>();
        image.color = Color.black;

        fadeCanvasGroup.alpha = 0;
    }

    public void TransitionToScene(string sceneName, string spawnPoint)
    {
        if (PauseMenu.Instance != null && PauseMenu.GameIsPaused)
        {
            PauseMenu.Instance.Resume();
        }
        Debug.Log($"SceneTransitionManager: Starting transition to {sceneName} with spawn point {spawnPoint}");
        if (sceneName == "Troy")
        {
            CleanupMainMenuObjects();
        }

        targetSceneName = sceneName;
        spawnPointIdentifier = spawnPoint;
        StartCoroutine(FadeAndLoadScene());
    }

    private IEnumerator FadeAndLoadScene()
    {
        yield return StartCoroutine(Fade(1f));

        if (targetSceneName == "MainMenu" || targetSceneName == "GameOver")
        {
            CleanupPersistentObjects();
        }

        SceneManager.LoadScene(targetSceneName);
        PlayerPrefs.SetString("SpawnPointIdentifier", spawnPointIdentifier);
        SceneManager.sceneLoaded += OnSceneLoaded; // Add sceneLoaded event handler
        yield return StartCoroutine(Fade(0f));
        CleanupDuplicateEventSystems();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"SceneTransitionManager: Scene {scene.name} loaded. Looking for spawn point {spawnPointIdentifier}");

        GameObject spawnPoint = GameObject.Find(spawnPointIdentifier);
        if (spawnPoint != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogWarning("SceneTransitionManager: Player not found in the scene. Instantiating new player.");
                player = Instantiate(GameManager.Instance.playerPrefab, spawnPoint.transform.position, Quaternion.identity);
                DontDestroyOnLoad(player);
            }
            else
            {
                player.transform.position = spawnPoint.transform.position;
                Debug.Log($"SceneTransitionManager: Player moved to spawn point {spawnPointIdentifier} at position {spawnPoint.transform.position}");
            }
        }
        else
        {
            Debug.LogWarning($"SceneTransitionManager: Spawn point {spawnPointIdentifier} not found in the scene.");
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void CleanupPersistentObjects()
    {
        Debug.Log("SceneTransitionManager: Cleaning up persistent objects");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Destroy(player);  // Ensure the player is properly destroyed
        }

        GameObject inventoryManager = GameObject.Find("InventoryManager");
        if (inventoryManager != null)
        {
            DontDestroyOnLoad(inventoryManager);
        }
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;
    }

    private void CleanupDuplicateEventSystems()
    {
        Debug.Log("SceneTransitionManager: Cleaning up duplicate EventSystems");
        var eventSystems = FindObjectsOfType<EventSystem>();
        if (eventSystems.Length > 1)
        {
            for (int i = 1; i < eventSystems.Length; i++)
            {
                Destroy(eventSystems[i].gameObject);
            }
        }
    }

    public void ResetChestStates()
    {
        Debug.Log("SceneTransitionManager: Resetting chest states");
        PlayerPrefs.DeleteAll();

        Chest[] chests = FindObjectsOfType<Chest>();
        foreach (Chest chest in chests)
        {
            chest.ResetChestState();
        }
    }

    public void StartNewGame(string sceneName, string spawnPoint)
    {
        Debug.Log($"SceneTransitionManager: Starting new game, scene: {sceneName}, spawn: {spawnPoint}");
        ResetChestStates();
        // Do not call TransitionToScene here, wait for cutscene to end.
        targetSceneName = sceneName;
        spawnPointIdentifier = spawnPoint;
    }

    public void CleanupMainMenuObjects()
    {
        GameObject mainMenu = GameObject.Find("MainMenu");
        if (mainMenu != null)
        {
            Debug.Log("Destroying MainMenu object");
            Destroy(mainMenu);
        }
        else
        {
            Debug.Log("MainMenu object not found for cleanup");
        }
    }

    private void HandleCutsceneComplete()
    {
        // The cutscene has ended, so start the fade and load the next scene
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            StartCoroutine(FadeAndLoadScene());
        }
    }
}
