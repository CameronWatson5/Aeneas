// This script was used to find out the size of the map.

using UnityEngine;
using UnityEngine.Tilemaps;

public class GridSizeFinder : MonoBehaviour
{
    public Tilemap tilemap;

    void Start()
    {
        if (tilemap == null)
        {
            Debug.LogError("Tilemap is not assigned.");
            return;
        }

        // Get the bounds of the tilemap in local space
        Bounds localBounds = tilemap.localBounds;
        Vector3 min = localBounds.min;
        Vector3 max = localBounds.max;

        // Calculate the size of the tilemap in units
        float width = max.x - min.x;
        float height = max.y - min.y;

        Debug.Log("Tilemap Size: " + width + " x " + height);
        Debug.Log("Min: " + min + ", Max: " + max);
    }
}