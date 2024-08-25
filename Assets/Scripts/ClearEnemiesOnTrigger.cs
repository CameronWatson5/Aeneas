// This script is used to clear all enemies when the player enter a box collider 2D trigger area.
// The purpose of this script is to ensure that the player is not overwhelmed by enemies.
// This script is triggered when a player enters or leaves a region in the Troy countryside.

using UnityEngine;
using UnityEngine.Tilemaps;

public class ClearEnemiesOnTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Clear enemies when the player enters a new area
            EnemyManager.Instance.ClearEnemies();
        }
    }
}

