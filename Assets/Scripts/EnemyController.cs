using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public int damageAmount = 10;
    public float damageCooldown = 1f;
    public LayerMask obstacleLayer;
    public float avoidanceRadius = 0.5f;
    public float pushForce = 5f; // Added push force

    private Transform player;
    private AeneasAttributes playerAttributes;
    private float lastDamageTime;
    private EnemyHealth health;
    private Rigidbody2D rb;
    private Collider2D enemyCollider;

    void Start()
    {
        health = GetComponent<EnemyHealth>();
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.freezeRotation = true;
        }

        if (enemyCollider == null)
        {
            Debug.LogError("No Collider2D found on the enemy. Please add a collider to the enemy prefab.");
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            playerAttributes = playerObject.GetComponent<AeneasAttributes>();
            Debug.Log("Player found and assigned.");
        }
        else
        {
            Debug.LogError("Player not found in the scene!");
        }

        // Ensure the obstacle layer includes the Collision layer
        obstacleLayer |= (1 << LayerMask.NameToLayer("Collision"));
    }

    void FixedUpdate()
    {
        if (player != null && health != null && !health.IsDead)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= detectionRange)
            {
                Vector2 moveDirection = CalculateMoveDirection();
                MoveEnemy(moveDirection);
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    Vector2 CalculateMoveDirection()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, avoidanceRadius);
        Vector2 avoidanceVector = Vector2.zero;

        foreach (Collider2D enemy in nearbyEnemies)
        {
            if (enemy != null && enemy.gameObject != gameObject)
            {
                Vector2 avoidDir = (transform.position - enemy.transform.position).normalized;
                avoidanceVector += avoidDir;
            }
        }

        return (direction + avoidanceVector.normalized).normalized;
    }

    void MoveEnemy(Vector2 direction)
    {
        Vector2 newPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
        RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, moveSpeed * Time.fixedDeltaTime, obstacleLayer);

        if (hit.collider != null)
        {
            // If there's an obstacle, move as close as we can to it
            newPosition = hit.point - direction * enemyCollider.bounds.extents.x;
        }

        rb.MovePosition(newPosition);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (health != null && !health.IsDead && collision.gameObject.CompareTag("Player") && Time.time >= lastDamageTime + damageCooldown)
        {
            if (playerAttributes != null)
            {
                playerAttributes.TakeDamage(damageAmount);
                lastDamageTime = Time.time;
            }

            // Apply push force to the player while in contact
            Rigidbody2D playerRigidbody = collision.collider.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
                playerRigidbody.AddForce(pushDirection * pushForce, ForceMode2D.Force);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Stop the pushing force when the player is no longer in contact
            Rigidbody2D playerRigidbody = collision.collider.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                playerRigidbody.velocity = Vector2.zero;
            }
        }
    }
}
