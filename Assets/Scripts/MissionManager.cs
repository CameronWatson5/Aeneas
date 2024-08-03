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
                // Ensure the cutscene is triggered for the indoor mission
                Debug.Log("MissionManager: Mission 1 completion check");
                if (IndoorCutsceneManager.Instance != null)
                {
                    IndoorCutsceneManager.Instance.TriggerCutsceneWithDelay(0.5f);
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

            if (currentMissionIndex == 1)
            {
                SetCompassTargetForMission2();
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
                Debug.LogError("MissionManager: MissionText is not assigned or all missions are completed.");
            }
        }
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
            GameObject troyHouse2Exit = GameObject.Find("TroyHouse2Exit");
            if (troyHouse2Exit != null)
            {
                compass.SetTarget(troyHouse2Exit.transform);
                Debug.Log("MissionManager: Compass target set to TroyHouse2Exit");
            }
            else
            {
                Debug.LogError("MissionManager: TroyHouse2Exit not found in the scene.");
            }
        }
        else
        {
            Debug.LogError("MissionManager: Compass not found in the scene.");
        }
    }
    public bool IsCurrentMission(int missionIndex)
    {
        return currentMissionIndex == missionIndex;
    }
    private void UpdateMissionText()
    {
        if (missionText != null && currentMissionIndex < missions.Count)
        {
            missionText.text = missions[currentMissionIndex];
            Debug.Log($"MissionManager: Mission text updated to: {missions[currentMissionIndex]}");
        }
        else
        {
            Debug.LogError("MissionManager: MissionText is not assigned or all missions are completed.");
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"MissionManager: Scene loaded - {scene.name}");
        if (scene.name == "Troy")
        {
            SetupTroyScene();
        }
    }

    private void SetupTroyScene()
    {
        Debug.Log("MissionManager: Setting up Troy scene");
        var missionPanel = GameObject.Find("MissionPanel");
        if (missionPanel != null)
        {
            missionText = missionPanel.GetComponentInChildren<TextMeshProUGUI>();
            if (missionText != null)
            {
                Debug.Log("MissionManager: MissionText found and assigned.");
                UpdateMissionText();
            }
            else
            {
                Debug.LogError("MissionManager: MissionText component not found in MissionPanel.");
            }
        }
        else
        {
            Debug.LogError("MissionManager: MissionPanel not found in the scene.");
        }

        if (currentMissionIndex == 1)
        {
            SetCompassTargetForMission2();
        }
    }

    
}