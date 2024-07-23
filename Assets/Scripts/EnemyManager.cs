using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private List<GameObject> activeEnemies = new List<GameObject>();

    public static EnemyManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterEnemy(GameObject enemy)
    {
        if (!activeEnemies.Contains(enemy))
        {
            activeEnemies.Add(enemy);
        }
    }

    public void UnregisterEnemy(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
    }

    public void ClearEnemies()
    {
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        activeEnemies.Clear();
    }
}