using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTransition : MonoBehaviour
{
    public string targetSceneName; // This can be modified in Unity for each object.
    public string spawnPointIdentifier; // Inside of a scene, an object is used to represent the spawn point of the player.

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"DoorTransition: Player entered trigger, transitioning to {targetSceneName} at {spawnPointIdentifier}");
            EnemyManager.Instance.ClearEnemies();
            SceneTransitionManager.Instance.TransitionToScene(targetSceneName, spawnPointIdentifier);
        }
    }
}