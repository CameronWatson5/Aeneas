using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int _currentHealth;

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

    public void TakeDamage(int damage)
    {
        if (IsDead) return;

        CurrentHealth -= damage;
        Debug.Log($"Enemy took {damage} damage. Current health: {CurrentHealth}");

        if (CurrentHealth <= 0)
        {
            Die();
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

        // Destroy the game object immediately
        Destroy(gameObject);
    }
}
