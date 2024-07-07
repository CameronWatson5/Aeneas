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
            // Move the player
            other.transform.position += (Vector3)playerMove;

            // Update camera target position
            cam.transform.position += (Vector3)cameraMove;

            // Update the tilemap for the camera
            cam.SetNewTilemap(newTilemap);
        }
    }
}