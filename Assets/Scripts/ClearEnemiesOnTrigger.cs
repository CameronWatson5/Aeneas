
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

