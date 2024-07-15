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
    private float nextSpawnTime;

    void Update()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);

        if (activeEnemies.Count < maxEnemies && Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        SetupEnemy(newEnemy);
        activeEnemies.Add(newEnemy);
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

        // Ensure other components are properly set up
        EnemyController controller = enemy.GetComponent<EnemyController>();
        if (controller == null)
        {
            Debug.LogError("EnemyController component not found on spawned enemy!");
        }

        Collider2D collider = enemy.GetComponent<Collider2D>();
        if (collider == null)
        {
            Debug.LogError("Collider2D component not found on spawned enemy!");
        }

        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on spawned enemy!");
        }
    }

    void HandleEnemyDeath(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
    }
}
