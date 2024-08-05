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
            "Find and defeat 3 Greek heroes.",
            "Go back home to sleep.",
            "Find your son, wife, and father in Troy.",
            "Escape from Troy."
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
            { "Father", false }
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
        }
    }

    public void CompleteCurrentMission()
    {
        Debug.Log($"MissionManager: Current mission {currentMissionIndex} completed!");
        if (currentMissionIndex < missions.Count - 1)
        {
            currentMissionIndex++;
            UpdateMissionText();

            if (currentMissionIndex == 1)
            {
                SetCompassTargetForMission2();
            }
            else if (currentMissionIndex == 2)
            {
                SetCompassTargetForMission3();
            }
            else if (currentMissionIndex == 3)
            {
                SetCompassTargetForMission4();
            }
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

    private void SetCompassTargetForMission2()
    {
        Debug.Log("MissionManager: Setting compass target for mission 2");
        if (compass == null)
        {
            compass = FindObjectOfType<Compass>();
        }

        if (compass != null)
        {
            GameObject troyHouse2Exit = GameObject.Find("TroyHouse2");
            if (troyHouse2Exit != null)
            {
                compass.SetTarget(troyHouse2Exit.transform);
                Debug.Log("MissionManager: Compass target set to TroyHouse2Exit");
            }
            else
            {
                Debug.LogWarning("MissionManager: TroyHouse2Exit not found in the scene.");
            }
        }
        else
        {
            Debug.LogWarning("MissionManager: Compass not found in the scene.");
        }
    }

    private void SetCompassTargetForMission3()
    {
        Debug.Log("MissionManager: Setting compass target for mission 3");
        if (compass == null)
        {
            compass = FindObjectOfType<Compass>();
        }

        if (compass != null)
        {
            GameObject familyLocation = GameObject.Find("FamilyLocation");
            if (familyLocation != null)
            {
                compass.SetTarget(familyLocation.transform);
                Debug.Log($"MissionManager: Compass target set to FamilyLocation at position {familyLocation.transform.position}");
            }
            else
            {
                Debug.LogWarning("MissionManager: FamilyLocation not found in the scene.");
            }
        }
        else
        {
            Debug.LogWarning("MissionManager: Compass not found in the scene.");
        }
    }


    private void SetCompassTargetForMission4()
    {
        Debug.Log("MissionManager: Setting compass target for mission 4");
        if (compass == null)
        {
            compass = FindObjectOfType<Compass>();
        }

        if (compass != null)
        {
            GameObject escapePoint = GameObject.Find("EscapePoint");
            if (escapePoint != null)
            {
                compass.SetTarget(escapePoint.transform);
                Debug.Log("MissionManager: Compass target set to EscapePoint");
            }
            else
            {
                Debug.LogWarning("MissionManager: EscapePoint not found in the scene.");
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
            SetCompassTargetForMission2();
        }
    }

    private void SetupIndoorScene()
    {
        UpdateMissionText();
        SetCompassTargetForMission3();
    }

    private void SetupTroySackScene()
    {
        UpdateMissionText();
        SetCompassTargetForMission3();
    }

    private void SetupCaveScene()
    {
        UpdateMissionText();
        SetCompassTargetForMission4();
    }
}