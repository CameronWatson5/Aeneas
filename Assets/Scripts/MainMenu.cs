using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }

    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject mainMenuElements;
    [SerializeField] private GameObject mainMenuCutscenePanel;

    private bool isStartButtonClicked = false;
    private bool isQuitButtonClicked = false;

    private void Awake()
    {
        Debug.Log("MainMenu: Awake called");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Ensure mainMenuElements and mainMenuCutscenePanel are set correctly
            if (mainMenuElements != null) mainMenuElements.transform.SetParent(transform);
            if (mainMenuCutscenePanel != null) mainMenuCutscenePanel.transform.SetParent(transform);
        }
        else if (Instance != this)
        {
            Debug.Log("Destroying duplicate MainMenu instance");
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        Debug.Log("MainMenu: Start called");

        // Check and log main components
        LogComponentStatus("mainMenuElements", mainMenuElements);
        LogComponentStatus("mainMenuCutscenePanel", mainMenuCutscenePanel);
        LogComponentStatus("startButton", startButton);
        LogComponentStatus("quitButton", quitButton);

        // Initialize and setup
        InitializeManagers();
        SetupButtons();
        SetupMainMenuCutscenePanel();

        // Log detailed information about UI elements
        LogRectTransformInfo("MainMenuElements", mainMenuElements);
        LogRectTransformInfo("StartButton", startButton?.gameObject);
        LogRectTransformInfo("QuitButton", quitButton?.gameObject);

        // Check and log Canvas information
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            LogCanvasInfo(canvas);
        }
        else
        {
            Debug.LogError("No Canvas found in parent hierarchy!");
        }

        // Check and log CanvasScaler information
        CanvasScaler scaler = canvas?.GetComponent<CanvasScaler>();
        if (scaler != null)
        {
            LogCanvasScalerInfo(scaler);
        }
        else
        {
            Debug.LogError("No CanvasScaler found!");
        }

        // Log information about all canvases in the scene
        LogAllCanvasesInfo();

        // Log main camera information
        LogMainCameraInfo();

        // Log hierarchy scale information
        LogHierarchyScaleInfo();
    }

    private void LogComponentStatus(string componentName, UnityEngine.Object component)
    {
        Debug.Log($"MainMenu: {componentName} is {(component != null ? "not null" : "null")}");
    }

    private void LogRectTransformInfo(string objectName, GameObject obj)
    {
        if (obj != null && obj.TryGetComponent<RectTransform>(out var rectTransform))
        {
            Debug.Log($"{objectName} active: {obj.activeSelf}");
            Debug.Log($"{objectName} position: {rectTransform.anchoredPosition}, size: {rectTransform.sizeDelta}");
        }
        else
        {
            Debug.LogWarning($"{objectName} is null or doesn't have a RectTransform component");
        }
    }

    private void LogCanvasInfo(Canvas canvas)
    {
        Debug.Log($"Canvas render mode: {canvas.renderMode}");
        Debug.Log($"Canvas scale factor: {canvas.scaleFactor}");
        Debug.Log($"Canvas reference pixels per unit: {canvas.referencePixelsPerUnit}");
    }

    private void LogCanvasScalerInfo(CanvasScaler scaler)
    {
        Debug.Log($"CanvasScaler scale mode: {scaler.uiScaleMode}");
        Debug.Log($"CanvasScaler reference resolution: {scaler.referenceResolution}");
        Debug.Log($"CanvasScaler screen match mode: {scaler.screenMatchMode}");
    }

    private void LogAllCanvasesInfo()
    {
        Canvas[] allCanvases = FindObjectsOfType<Canvas>();
        foreach (Canvas c in allCanvases)
        {
            Debug.Log($"Canvas name: {c.name}, Sort Order: {c.sortingOrder}, Render Mode: {c.renderMode}");
        }
    }

    private void LogMainCameraInfo()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Debug.Log($"Main Camera position: {mainCamera.transform.position}, rotation: {mainCamera.transform.rotation}");
            Debug.Log($"Main Camera orthographic: {mainCamera.orthographic}, field of view: {mainCamera.fieldOfView}");
        }
        else
        {
            Debug.LogError("No Main Camera found in the scene!");
        }
    }

    private void LogHierarchyScaleInfo()
    {
        Transform current = transform;
        while (current != null)
        {
            Debug.Log($"Object {current.name} scale: {current.localScale}");
            current = current.parent;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            ResetMainMenu();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void InitializeManagers()
    {
        if (MissionManager.Instance == null)
        {
            GameObject missionManager = new GameObject("MissionManager");
            missionManager.AddComponent<MissionManager>();
            DontDestroyOnLoad(missionManager);
        }
    }

    public void SetupButtons()
    {
        Debug.Log("MainMenu: SetupButtons called");
        if (startButton == null)
        {
            startButton = transform.Find("MainMenuElements/StartButton")?.GetComponent<Button>();
        }
        if (quitButton == null)
        {
            quitButton = transform.Find("MainMenuElements/QuitButton")?.GetComponent<Button>();
        }

        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(OnStartButtonClick);
            Debug.Log("MainMenu: Start button listener added");
        }
        else
        {
            Debug.LogWarning("MainMenu: StartButton reference is missing");
        }

        if (quitButton != null)
        {
            quitButton.onClick.RemoveAllListeners();
            quitButton.onClick.AddListener(OnQuitButtonClick);
            Debug.Log("MainMenu: Quit button listener added");
        }
        else
        {
            Debug.LogWarning("MainMenu: QuitButton reference is missing");
        }
    }

    private void OnStartButtonClick()
    {
        if (!isStartButtonClicked)
        {
            isStartButtonClicked = true;
            AudioManager.Instance.PlayButtonClickSound();
            StartGame();
        }
    }

    private void OnQuitButtonClick()
    {
        if (!isQuitButtonClicked)
        {
            isQuitButtonClicked = true;
            AudioManager.Instance.PlayButtonClickSound();
            QuitGame();
        }
    }

    private void SetupMainMenuCutscenePanel()
    {
        if (mainMenuCutscenePanel != null)
        {
            mainMenuCutscenePanel.SetActive(false);
            Debug.Log("MainMenu: Main menu cutscene panel set to inactive");
        }
        else
        {
            Debug.LogWarning("MainMenu: MainMenuCutscenePanel reference is missing");
        }
    }

    public void StartGame()
    {
        Debug.Log("MainMenu: StartGame called");
        Debug.Log($"MainMenuElements position: {mainMenuElements.GetComponent<RectTransform>().position}, size: {mainMenuElements.GetComponent<RectTransform>().sizeDelta}");

        if (startButton != null)
        {
            Debug.Log($"StartButton position: {startButton.GetComponent<RectTransform>().position}, size: {startButton.GetComponent<RectTransform>().sizeDelta}");
        }
        Debug.Log($"MainMenuElements active: {mainMenuElements.activeSelf}");
        if (mainMenuElements != null)
        {
            mainMenuElements.SetActive(false);
            Debug.Log("MainMenu: Main menu elements deactivated");
        }
        else
        {
            Debug.LogWarning("MainMenu: mainMenuElements reference is missing");
        }

        if (mainMenuCutscenePanel != null)
        {
            mainMenuCutscenePanel.SetActive(true);
            Debug.Log("MainMenu: Main menu cutscene panel activated");

            MainMenuCutsceneManager cutsceneManager = mainMenuCutscenePanel.GetComponent<MainMenuCutsceneManager>();
            if (cutsceneManager != null)
            {
                cutsceneManager.enabled = true;
                Debug.Log("MainMenu: MainMenuCutsceneManager enabled");
            }
            else
            {
                Debug.LogWarning("MainMenu: MainMenuCutsceneManager component is missing on the CutscenePanel");
            }
        }
        else
        {
            Debug.LogWarning("MainMenu: MainMenuCutscenePanel reference is missing");
        }

        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            Debug.Log($"Canvas render mode: {canvas.renderMode}");
            Debug.Log($"Canvas scale factor: {canvas.scaleFactor}");
            Debug.Log($"Canvas reference pixels per unit: {canvas.referencePixelsPerUnit}");
        }
        else
        {
            Debug.LogError("No Canvas found in parent hierarchy!");
        }
    }

    public void QuitGame()
    {
        Debug.Log("MainMenu: QuitGame called");
        Application.Quit();
    }

    public void ResetMainMenu()
    {
        if (mainMenuElements == null)
        {
            mainMenuElements = transform.Find("MainMenuElements")?.gameObject;
            if (mainMenuElements == null) Debug.LogError("MainMenuElements not found as a child of MainMenu.");
        }

        if (mainMenuCutscenePanel == null)
        {
            mainMenuCutscenePanel = transform.Find("MainMenuCutscenePanel")?.gameObject;
            if (mainMenuCutscenePanel == null) Debug.LogError("MainMenuCutscenePanel not found as a child of MainMenu.");
        }

        if (mainMenuElements != null)
        {
            mainMenuElements.SetActive(true);
        }
        if (mainMenuCutscenePanel != null)
        {
            mainMenuCutscenePanel.SetActive(false);
            MainMenuCutsceneManager cutsceneManager = mainMenuCutscenePanel.GetComponent<MainMenuCutsceneManager>();
            if (cutsceneManager != null)
            {
                cutsceneManager.ResetCutscene();
            }
        }
        SetupButtons();
        isStartButtonClicked = false;
        isQuitButtonClicked = false;
    }
}
