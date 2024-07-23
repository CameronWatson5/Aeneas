using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public int maxEnemies = 10;
    public float spawnInterval = 5f;

    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    void Update()
    {
        CleanupEnemies();
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (activeEnemies.Count < maxEnemies)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        SetupEnemy(newEnemy);
        activeEnemies.Add(newEnemy);
        EnemyManager.Instance.RegisterEnemy(newEnemy);
    }

    void SetupEnemy(GameObject enemy)
    {
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.OnEnemyDeath += HandleEnemyDeath;
        }
        else
        {
            Debug.LogError("EnemyHealth component not found on spawned enemy!");
        }

        if (enemy.GetComponent<EnemyController>() == null)
        {
            Debug.LogError("EnemyController component not found on spawned enemy!");
        }

        if (enemy.GetComponent<Collider2D>() == null)
        {
            Debug.LogError("Collider2D component not found on spawned enemy!");
        }

        if (enemy.GetComponent<Rigidbody2D>() == null)
        {
            Debug.LogError("Rigidbody2D component not found on spawned enemy!");
        }
    }

    void CleanupEnemies()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);
    }

    void HandleEnemyDeath(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
        EnemyManager.Instance.UnregisterEnemy(enemy);
    }
}
