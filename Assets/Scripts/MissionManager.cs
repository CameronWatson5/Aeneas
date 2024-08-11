using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI missionText;
    private int currentMissionIndex = 0;
    private List<string> missions;
    public Dictionary<string, bool> missionTargets;
    public int CurrentMissionIndex => currentMissionIndex;
    private Compass compass;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeMissions();
            InitializeMissionTargets();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeMissions()
    {
        missions = new List<string>
        {
            "Mission 1: Find and defeat 3 Greek heroes.",
            "Mission 2: Go back home to sleep in your house in Troy. Your house is near the gate and has a fountain in front of it.",
            "Mission 3: Find your son, wife, and father in Troy.",
            "Mission 4: Find a way to escape from Troy.",
            "Mission 5: Find Queen Dido in Carthage.",
            "Mission 6: Find your late father in Hades.",
            "Mission 7: Defeat Turnus in Italy."
        };
        Debug.Log($"MissionManager: Initialized {missions.Count} missions");
    }

    private void InitializeMissionTargets()
    {
        missionTargets = new Dictionary<string, bool>
        {
            { "GreekHero1", false },
            { "GreekHero2", false },
            { "GreekHero3", false },
            { "Son", false },
            { "Wife", false },
            { "Father", false },
            { "QueenDido", false },
            { "FatherInHades", false },
            { "Turnus", false }
        };
        Debug.Log($"MissionManager: Initialized {missionTargets.Count} mission targets");
    }

    public void SetMissionTargetStatus(string targetName, bool isDefeated)
    {
        if (missionTargets.ContainsKey(targetName))
        {
            missionTargets[targetName] = isDefeated;
            Debug.Log($"MissionManager: {targetName} status set to {isDefeated}");
            CheckMissionCompletion();
        }
        else
        {
            Debug.LogError($"MissionManager: Target {targetName} not found in missionTargets.");
        }
    }

    private void CheckMissionCompletion()
    {
        Debug.Log($"MissionManager: Checking completion for mission index {currentMissionIndex}");
        switch (currentMissionIndex)
        {
            case 0:
                if (missionTargets["GreekHero1"] && missionTargets["GreekHero2"] && missionTargets["GreekHero3"])
                {
                    CompleteCurrentMission();
                }
                break;
            case 1:
                CompleteCurrentMission();
                break;
            case 2:
                if (missionTargets["Son"] && missionTargets["Wife"] && missionTargets["Father"])
                {
                    CompleteCurrentMission();
                }
                break;
            case 3:
                CompleteCurrentMission();
                break;
            case 4:
                if (missionTargets["QueenDido"])
                {
                    CompleteCurrentMission();
                }
                break;
            case 5:
                if (missionTargets["FatherInHades"])
                {
                    CompleteCurrentMission();
                }
                break;
            case 6:
                if (missionTargets["Turnus"])
                {
                    CompleteCurrentMission();
                }
                break;
        }
    }

    public void CompleteCurrentMission()
    {
        Debug.Log($"MissionManager: Current mission {currentMissionIndex} completed!");
        if (currentMissionIndex < missions.Count - 1)
        {
            currentMissionIndex++;
            UpdateMissionText();
            SetCompassTargetForCurrentMission();
        }
        else
        {
            if (missionText != null)
            {
                missionText.text = "All missions complete!";
            }
            else
            {
                Debug.LogWarning("MissionManager: MissionText is not assigned or all missions are completed.");
            }
        }
    }

    public void ResetMissionIndex()
    {
        currentMissionIndex = 0;
        InitializeMissionTargets();
        UpdateMissionText();
        Debug.Log("MissionManager: Mission index reset to 0");
    }

    private void SetCompassTargetForCurrentMission()
    {
        Debug.Log("MissionManager: Setting compass target for current mission");
        if (compass == null)
        {
            compass = FindObjectOfType<Compass>();
        }

        if (compass != null)
        {
            GameObject targetObject = null;

            switch (currentMissionIndex)
            {
                case 1:
                    targetObject = GameObject.Find("TroyHouse2");
                    break;
                case 2:
                    targetObject = GameObject.Find("FamilyLocation");
                    break;
                case 3:
                    targetObject = GameObject.Find("EscapePoint");
                    break;
                case 4:
                    targetObject = GameObject.Find("QueenDidoLocation");
                    break;
                case 5:
                    targetObject = GameObject.Find("FatherInHadesLocation");
                    break;
                case 6:
                    targetObject = GameObject.Find("TurnusLocation");
                    break;
            }

            if (targetObject != null)
            {
                compass.SetTarget(targetObject.transform);
                Debug.Log($"MissionManager: Compass target set to {targetObject.name}");
            }
            else
            {
                Debug.LogWarning("MissionManager: Target object not found in the scene.");
            }
        }
        else
        {
            Debug.LogWarning("MissionManager: Compass not found in the scene.");
        }
    }

    private void UpdateMissionText()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            return; // Don't update mission text in MainMenu
        }

        if (missionText == null)
        {
            FindAndAssignMissionText();
        }

        if (missionText != null && currentMissionIndex < missions.Count)
        {
            missionText.text = missions[currentMissionIndex];
            Debug.Log($"MissionManager: Mission text updated to: {missions[currentMissionIndex]}");
        }
        else
        {
            Debug.LogWarning("MissionManager: Unable to update mission text.");
        }
    }

    private void FindAndAssignMissionText()
    {
        GameObject missionPanel = GameObject.Find("MissionPanel");
        if (missionPanel != null)
        {
            missionText = missionPanel.GetComponentInChildren<TextMeshProUGUI>();
            if (missionText != null)
            {
                Debug.Log("MissionManager: MissionText found and assigned.");
            }
            else
            {
                Debug.LogWarning("MissionManager: MissionText component not found in MissionPanel.");
            }
        }
        else
        {
            Debug.LogWarning("MissionManager: MissionPanel not found in the scene.");
        }
    }

    public bool IsCurrentMission(int missionIndex)
    {
        return currentMissionIndex == missionIndex;
    }

    public void TransitionToGameOver()
    {
        Debug.Log("MissionManager: Transitioning to GameOver scene");
        SceneManager.LoadScene("GameOver");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"MissionManager: Scene loaded - {scene.name}");
        if (scene.name == "MainMenu")
        {
            ResetMissionIndex();
        }
        else
        {
            FindAndAssignMissionText();
            UpdateMissionText();

            if (scene.name == "Troy")
            {
                SetupTroyScene();
            }
            else if (scene.name == "Indoor")
            {
                SetupIndoorScene();
            }
            else if (scene.name == "TroySack")
            {
                SetupTroySackScene();
            }
            else if (scene.name == "Cave")
            {
                SetupCaveScene();
            }
            else if (scene.name == "Carthage")
            {
                SetupCarthageScene();
            }
            else if (scene.name == "Hades")
            {
                SetupHadesScene();
            }
            else if (scene.name == "Italy")
            {
                SetupItalyScene();
            }
        }
    }

    private void SetupTroyScene()
    {
        UpdateMissionText();
        if (currentMissionIndex == 0)
        {
            if (compass == null)
            {
                compass = FindObjectOfType<Compass>();
            }
            if (compass != null)
            {
                compass.FindGreekHeroes();
            }
        }
        else
        {
            SetCompassTargetForCurrentMission();
        }
    }

    private void SetupIndoorScene()
    {
        UpdateMissionText();
        SetCompassTargetForCurrentMission();
    }

    private void SetupTroySackScene()
    {
        UpdateMissionText();
        SetCompassTargetForCurrentMission();
    }

    private void SetupCaveScene()
    {
        UpdateMissionText();
        SetCompassTargetForCurrentMission();
    }

    private void SetupCarthageScene()
    {
        UpdateMissionText();
        SetCompassTargetForCurrentMission();
    }

    private void SetupHadesScene()
    {
        UpdateMissionText();
        SetCompassTargetForCurrentMission();
    }

    private void SetupItalyScene()
    {
        UpdateMissionText();
        SetCompassTargetForCurrentMission();
    }
}
