// This script is for the camera object.
// The camera object has a target (Aeneas, the main character)
// The camera then follows this target.
// FollowSpeed determines how quickly the target is followed (This can be edited in Unity too)
// The tilemap variable is a reference to the tilemap that Aeneas is on,
// this prevents the camera from going out of bounds.
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Target = player (Aeneas)
    public Vector3 offset = new Vector3(0, 0, -10); // Offset from player
    public float followSpeed = 2f; // This is used to determine how quickly the camera follows the player
    public Tilemap tilemap; // Tilemap is the 2D map
    
    private Camera cam;
    private Vector3 minBounds;
    private Vector3 maxBounds;

    void Start()
    {
        cam = GetComponent<Camera>();
        CalculateBounds();
    }

    void CalculateBounds()
    {
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

    void LateUpdate()
    {
        Vector3 targetPosition = target.position + offset;
        
        // Moves camera towards target
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        
        // Ensures camera stays in bounds
        float clampedX = Mathf.Clamp(smoothedPosition.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(smoothedPosition.y, minBounds.y, maxBounds.y);
        
        transform.position = new Vector3(clampedX, clampedY, smoothedPosition.z);
    }
}