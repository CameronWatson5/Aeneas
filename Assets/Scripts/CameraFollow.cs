// This script is used to control the camera and make it follow the player.
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 0, -10); // Offset from player
    public float followSpeed = 2f; // This is used to determine how quickly the camera follows the player
    public Tilemap tilemap; // Tilemap is the 2D map
    
    public Transform target; // Target = player (Aeneas)
    private Camera cam;
    private Vector3 minBounds;
    private Vector3 maxBounds;
    public bool isNewScene = true; 

    void Start()
    {
        cam = GetComponent<Camera>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        FindPlayer();
        FindTilemap(); // Ensure Tilemap is found at the start
        CalculateBounds();
        Debug.Log("CameraFollow Start: Camera initialized.");
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayer();
        FindTilemap();
        CalculateBounds();
        isNewScene = true; // Set to true when a new scene is loaded
        Debug.Log($"OnSceneLoaded: New scene loaded - {scene.name}");
    }

    public void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
            Debug.Log($"FindPlayer: Player found - {player.name}");
        }
        else
        {
            Debug.LogWarning("FindPlayer: Player not found in the scene.");
        }
    }

    public void FindTilemap()
    {
        if (tilemap == null)
        {
            tilemap = FindObjectOfType<Tilemap>();
        }

        if (tilemap == null)
        {
            Debug.LogWarning("FindTilemap: Tilemap not found in the scene.");
        }
        else
        {
            Debug.Log($"FindTilemap: Tilemap found - {tilemap.name}");
        }
    }

    public void CalculateBounds()
    {
        if (tilemap == null)
        {
            Debug.LogWarning("CalculateBounds: Tilemap is not set in CameraFollow script.");
            return;
        }

        // Get the world space bounds of the tilemap
        Vector3 tilemapMin = tilemap.localBounds.min;
        Vector3 tilemapMax = tilemap.localBounds.max;
        
        // Calculate the camera's half-width and half-height
        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = cam.aspect * camHalfHeight;

        // Adjust bounds to keep the camera within the tilemap
        minBounds = tilemapMin + new Vector3(camHalfWidth, camHalfHeight, 0);
        maxBounds = tilemapMax - new Vector3(camHalfWidth, camHalfHeight, 0);

        Debug.Log($"CalculateBounds: minBounds={minBounds}, maxBounds={maxBounds}");
    }

    public void SetNewTilemap(Tilemap newTilemap)
    {
        tilemap = newTilemap;
        CalculateBounds();
        Debug.Log($"SetNewTilemap: New tilemap set - {newTilemap.name}");
    }

    void LateUpdate()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu" || SceneManager.GetActiveScene().name == "GameOver")
        {
            // Don't try to follow the player in these scenes
            return;
        }

        if (target == null)
        {
            FindPlayer();
            return;
        }

        if (tilemap == null)
        {
            FindTilemap();
            return;
        }

        Vector3 targetPosition = target.position + offset;

        if (isNewScene)
        {
            // Instantly center camera on player when entering a new scene
            Vector3 instantPosition = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
            transform.position = ClampCamera(instantPosition);
            isNewScene = false; // Reset the flag
            Debug.Log($"LateUpdate: Instantly centered on player - {target.position}");
        }
        else
        {
            // Normal smooth camera movement during gameplay
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            transform.position = ClampCamera(smoothedPosition);
            //Debug.Log($"LateUpdate: Smoothed position - {smoothedPosition}");
        }
    }

    public Vector3 ClampCamera(Vector3 position)
    {
        // Ensures camera stays in bounds
        float clampedX = Mathf.Clamp(position.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(position.y, minBounds.y, maxBounds.y);
        return new Vector3(clampedX, clampedY, position.z);
    }
}
