// This script is used to control the player and scene transitions.

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject playerPrefab;
    private GameObject playerInstance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePlayer();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameOver" || scene.name == "MainMenu")
        {
            DestroyPlayerInstance();
        }
        else
        {
            EnsurePlayerInScene(scene.name);
        }
    }

    private void InitializePlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("GameManager: playerPrefab is not assigned!");
            return;
        }

        if (playerInstance == null)
        {
            playerInstance = Instantiate(playerPrefab);
            DontDestroyOnLoad(playerInstance);
            Debug.Log("GameManager: New player instance created");

            AeneasAttributes playerAttributes = playerInstance.GetComponent<AeneasAttributes>();
            if (playerAttributes != null)
            {
                playerAttributes.ResetAttributes();
            }
        }
    }

    private void EnsurePlayerInScene(string sceneName)
    {
        if (playerInstance == null)
        {
            Debug.LogWarning("GameManager: Player instance not found, reinitializing player.");
            InitializePlayer();
        }
        else
        {
            MovePlayerToSpawnPoint(sceneName);
        }
    }

    private void MovePlayerToSpawnPoint(string sceneName)
    {
        string spawnPointName = sceneName + "SpawnPoint";
        GameObject spawnPoint = GameObject.Find(spawnPointName);

        if (spawnPoint != null)
        {
            playerInstance.transform.position = spawnPoint.transform.position;
            Debug.Log($"GameManager: Player moved to spawn point {spawnPointName} at position {spawnPoint.transform.position}");
        }
        else
        {
            Debug.LogWarning($"GameManager: Spawn point {spawnPointName} not found in the scene.");
        }
    }

    public void GameOver()
    {
        DeactivatePlayer();
        SceneTransitionManager.Instance?.TransitionToScene("GameOver", "");
    }

    public void RestartGame()
    {
        Debug.Log("GameManager: Restarting game");

        MissionManager.Instance?.ResetMissionIndex();

        ResetPlayer();

        SceneTransitionManager.Instance?.TransitionToScene("Troy", "DefaultSpawnPoint");
    }

    private void ResetPlayer()
    {
        InitializePlayer();
        InventoryManager.Instance.ApplyAllEquippedItemEffects();
    }

    private void DestroyPlayerInstance()
    {
        if (playerInstance != null)
        {
            Destroy(playerInstance);
            playerInstance = null;
        }
    }

    public void DeactivatePlayer()
    {
        if (playerInstance != null)
        {
            playerInstance.SetActive(false);
        }
    }

    public void ActivatePlayer()
    {
        if (playerInstance != null)
        {
            playerInstance.SetActive(true);
        }
    }

    public void PlayerDied()
    {
        SceneTransitionManager.Instance?.TransitionToScene("GameOver", "");
    }
}
