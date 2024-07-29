// This script is used to change the location of the player and the camera.
// This can be used to create transitions between different areas in the game.
// For example, Troy inside of the city and Troy countryside tile maps.

using UnityEngine;
using UnityEngine.Tilemaps;

public class MapChange : MonoBehaviour
{
    public Vector2 cameraMove;
    public Vector2 playerMove;
    public Tilemap newTilemap;  // Reference to the new tilemap

    private CameraFollow cam;

    void Start()
    {
        cam = Camera.main.GetComponent<CameraFollow>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Clear enemies when the player enters a new area
            EnemyManager.Instance.ClearEnemies();

            // Move the player
            other.transform.position += (Vector3)playerMove;

            // Update the tilemap for the camera
            cam.SetNewTilemap(newTilemap);

            // Ensure camera re-centers on the player
            cam.FindPlayer();
            cam.CalculateBounds();
            cam.isNewScene = true; // Force camera to center on player

            // Force camera to center on player immediately
            ForceCenterCameraOnPlayer();

            // Log the changes
            Debug.Log($"OnTriggerEnter2D: Player moved to {other.transform.position}");
            Debug.Log($"OnTriggerEnter2D: Camera moved to {cam.transform.position}");
            Debug.Log($"OnTriggerEnter2D: Tilemap set to {newTilemap.name}");
        }
    }

    private void ForceCenterCameraOnPlayer()
    {
        if (cam.target != null)
        {
            Vector3 targetPosition = cam.target.position + cam.offset;
            cam.transform.position = cam.ClampCamera(targetPosition);
            Debug.Log($"ForceCenterCameraOnPlayer: Camera centered on {cam.transform.position}");
        }
        else
        {
            Debug.LogWarning("ForceCenterCameraOnPlayer: Camera target is null.");
        }
    }
}

