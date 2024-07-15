using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public int damageAmount = 10;
    public float damageCooldown = 1f;
    public LayerMask obstacleLayer; 

    private Transform player;
    private AeneasAttributes playerAttributes;
    private float lastDamageTime;
    private EnemyHealth health;
    private Collider2D enemyCollider;

    void Start()
    {
        health = GetComponent<EnemyHealth>();
        enemyCollider = GetComponent<Collider2D>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            playerAttributes = playerObject.GetComponent<AeneasAttributes>();
        }
        else
        {
            Debug.LogError("Player not found in the scene!");
        }
    }

    void Update()
    {
        if (player != null && !health.IsDead)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            
            if (distanceToPlayer <= detectionRange)
            {
                MoveTowardsPlayer();
            }
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 newPosition = (Vector2)transform.position + direction * moveSpeed * Time.deltaTime;

        // Check for collisions before moving
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, enemyCollider.bounds.extents.x, direction, moveSpeed * Time.deltaTime, obstacleLayer);

        if (!hit)
        {
            transform.position = newPosition;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!health.IsDead && collision.gameObject.CompareTag("Player") && Time.time >= lastDamageTime + damageCooldown)
        {
            if (playerAttributes != null)
            {
                playerAttributes.TakeDamage(damageAmount);
                lastDamageTime = Time.time;
            }
        }
    }
}