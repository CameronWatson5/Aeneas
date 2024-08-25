// This script is used in the Indoor scene.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndoorManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public string[] spawnPointIdentifiers;

    private void Start()
    {
        PositionPlayer();
    }

    private void PositionPlayer()
    {
        if (PlayerPrefs.HasKey("SpawnPointIdentifier"))
        {
            string targetSpawnPoint = PlayerPrefs.GetString("SpawnPointIdentifier");
            int index = System.Array.IndexOf(spawnPointIdentifiers, targetSpawnPoint);
            
            if (index != -1 && index < spawnPoints.Length)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    player.transform.position = spawnPoints[index].position;
                }
            }
            
            PlayerPrefs.DeleteKey("SpawnPointIdentifier");
        }
    }
}