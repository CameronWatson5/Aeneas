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
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializePlayer();
        SceneManager.sceneLoaded += OnSceneLoaded;
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
        else if (scene.name == "Troy")
        {
            InitializePlayer();
        }
    }

    private void InitializePlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("GameManager: playerPrefab is not assigned!");
            return;
        }

        DestroyPlayerInstance();

        playerInstance = Instantiate(playerPrefab);
        DontDestroyOnLoad(playerInstance);
        Debug.Log("GameManager: New player instance created");

        AeneasAttributes playerAttributes = playerInstance.GetComponent<AeneasAttributes>();
        if (playerAttributes != null)
        {
            playerAttributes.ResetAttributes();
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
