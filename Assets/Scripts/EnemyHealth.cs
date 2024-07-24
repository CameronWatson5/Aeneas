using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int _currentHealth;
    public float knockbackDuration = 0.2f; // Duration to disable movement
    public GameObject[] droppableItems; // Array of possible droppable items
    public float dropChance = 0.2f; // 20% chance to drop an item

    public event Action<GameObject> OnEnemyDeath;

    public int CurrentHealth
    {
        get { return _currentHealth; }
        private set { _currentHealth = Mathf.Clamp(value, 0, maxHealth); }
    }

    public bool IsDead { get; private set; }

    void Start()
    {
        CurrentHealth = maxHealth;
        IsDead = false;
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection, float knockbackForce)
    {
        if (IsDead) return;

        CurrentHealth -= damage;
        Debug.Log($"Enemy took {damage} damage. Current health: {CurrentHealth}");

        if (CurrentHealth <= 0)
        {
            Die();
        }
        else
        {
            Debug.Log("Enemy should be knocked back");
            StartCoroutine(ApplyKnockback(knockbackDirection, knockbackForce));
        }
    }

    private void Die()
    {
        if (IsDead) return;

        IsDead = true;
        Debug.Log("Enemy died");

        // Immediately disable collider
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Disable the renderer
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }

        // Disable the EnemyController
        EnemyController controller = GetComponent<EnemyController>();
        if (controller != null)
        {
            controller.enabled = false;
        }

        // Trigger the death event
        OnEnemyDeath?.Invoke(gameObject);

        // Drop an item
        DropItem();

        // Destroy the game object immediately
        Destroy(gameObject);
    }

    private void DropItem()
    {
        if (droppableItems.Length > 0 && Random.value <= dropChance)
        {
            int randomIndex = Random.Range(0, droppableItems.Length);
            Instantiate(droppableItems[randomIndex], transform.position, Quaternion.identity);
        }
    }

    private IEnumerator ApplyKnockback(Vector2 direction, float force)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        EnemyController controller = GetComponent<EnemyController>();
        
        if (rb != null)
        {
            if (controller != null)
            {
                controller.enabled = false; // Temporarily disable enemy movement
            }
            
            rb.AddForce(direction * force, ForceMode2D.Impulse);
            Debug.Log($"Knockback applied with direction {direction} and force {force}");
            Debug.Log($"Enemy velocity after knockback: {rb.velocity}");

            yield return new WaitForSeconds(knockbackDuration); // Wait for knockback duration

            if (controller != null)
            {
                controller.enabled = true; // Re-enable enemy movement
            }
        }
        else
        {
            Debug.LogError("Rigidbody2D not found on enemy for knockback");
        }
    }
}
