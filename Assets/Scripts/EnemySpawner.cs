// This script is used to spawn enemies.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public int maxEnemies = 10;
    public float spawnInterval = 5f;
    public float spawnRange = 50f; 
    public bool showRange = true; 

    private List<GameObject> activeEnemies = new List<GameObject>();
    private Coroutine spawnCoroutine;
    private Transform playerTransform;
    private LineRenderer rangeIndicator;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found in the scene!");
        }

        rangeIndicator = GetComponent<LineRenderer>();
        if (rangeIndicator != null)
        {
            rangeIndicator.positionCount = 0; 

            rangeIndicator.startWidth = 0.1f;
            rangeIndicator.endWidth = 0.1f;
            rangeIndicator.loop = true;
            rangeIndicator.material = new Material(Shader.Find("Sprites/Default")) { color = Color.red };

            DrawRangeIndicator();
            rangeIndicator.enabled = showRange; 
        }

        StartSpawning();
    }

    void Update()
    {
        CleanupEnemies();

        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleRangeIndicator();
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (activeEnemies.Count < maxEnemies && playerTransform != null)
            {
                Transform spawnPoint = GetSpawnPointInRange();
                if (spawnPoint != null)
                {
                    SpawnEnemy(spawnPoint);
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy(Transform spawnPoint)
    {
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

    Transform GetSpawnPointInRange()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (playerTransform != null && Vector2.Distance(spawnPoint.position, playerTransform.position) <= spawnRange)
            {
                Renderer renderer = spawnPoint.GetComponent<Renderer>();
                if (renderer != null && !renderer.isVisible)
                {
                    return spawnPoint;
                }
            }
        }
        return null;
    }

    void StartSpawning()
    {
        spawnCoroutine = StartCoroutine(SpawnEnemies());
    }

    void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    public void ToggleRangeIndicator()
    {
        showRange = !showRange;
        if (rangeIndicator != null)
        {
            rangeIndicator.enabled = showRange;
        }
    }

    void DrawRangeIndicator()
    {
        int segments = 50;
        float angle = 0f;

        rangeIndicator.positionCount = segments + 1; 

        for (int i = 0; i < segments + 1; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * spawnRange;
            float y = Mathf.Cos(Mathf.Deg2Rad * angle) * spawnRange;

            rangeIndicator.SetPosition(i, new Vector3(x, y, 0));

            angle += (360f / segments);
        }
    }
}
