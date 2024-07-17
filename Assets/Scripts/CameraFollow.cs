// This script is for the camera object.
// The camera object has a target (Aeneas, the main character)
// The camera then follows this target.
// FollowSpeed determines how quickly the target is followed (This can be edited in Unity too)
// The tilemap variable is a reference to the tilemap that Aeneas is on,
// this prevents the camera from going out of bounds.
using UnityEngine;
using UnityEngine.Tilemaps;
<<<<<<< HEAD
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
=======

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Target = player (Aeneas)
>>>>>>> gitlab/main
    public Vector3 offset = new Vector3(0, 0, -10); // Offset from player
    public float followSpeed = 2f; // This is used to determine how quickly the camera follows the player
    public Tilemap tilemap; // Tilemap is the 2D map
    
<<<<<<< HEAD
    private Transform target; // Target = player (Aeneas)
    private Camera cam;
    private Vector3 minBounds;
    private Vector3 maxBounds;
    private bool isNewScene = true; // New variable to track scene changes
=======
    private Camera cam;
    private Vector3 minBounds;
    private Vector3 maxBounds;
>>>>>>> gitlab/main

    void Start()
    {
        cam = GetComponent<Camera>();
<<<<<<< HEAD
        SceneManager.sceneLoaded += OnSceneLoaded;
        FindPlayer();
        CalculateBounds();
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
    }

    void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found in the scene.");
        }
    }

    void FindTilemap()
    {
        tilemap = FindObjectOfType<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogWarning("Tilemap not found in the scene.");
        }
    }

    public void CalculateBounds()
    {
        if (tilemap == null)
        {
            Debug.LogWarning("Tilemap is not set in CameraFollow script.");
            return;
        }

=======
        CalculateBounds();
    }

    void CalculateBounds()
    {
>>>>>>> gitlab/main
        // Get the world space bounds of the tilemap
        Vector3 tilemapMin = tilemap.localBounds.min;
        Vector3 tilemapMax = tilemap.localBounds.max;
        
        // Calculate the camera's half-width and half-height
        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = cam.aspect * camHalfHeight;

        // Adjust bounds to keep the camera within the tilemap
        minBounds = tilemapMin + new Vector3(camHalfWidth, camHalfHeight, 0);
        maxBounds = tilemapMax - new Vector3(camHalfWidth, camHalfHeight, 0);
    }

<<<<<<< HEAD
    public void SetNewTilemap(Tilemap newTilemap)
    {
        tilemap = newTilemap;
        CalculateBounds();
    }

    void LateUpdate()
    {
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
        }
        else
        {
            // Normal smooth camera movement during gameplay
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            transform.position = ClampCamera(smoothedPosition);
        }
    }

    Vector3 ClampCamera(Vector3 position)
    {
        // Ensures camera stays in bounds
        float clampedX = Mathf.Clamp(position.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(position.y, minBounds.y, maxBounds.y);
        return new Vector3(clampedX, clampedY, position.z);
=======
    void LateUpdate()
    {
        Vector3 targetPosition = target.position + offset;
        
        // Moves camera towards target
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        
        // Ensures camera stays in bounds
        float clampedX = Mathf.Clamp(smoothedPosition.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(smoothedPosition.y, minBounds.y, maxBounds.y);
        
        transform.position = new Vector3(clampedX, clampedY, smoothedPosition.z);
>>>>>>> gitlab/main
    }
}