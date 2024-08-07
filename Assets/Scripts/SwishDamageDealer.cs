using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwishDamageDealer : MonoBehaviour
{
    private int damage;
    private float knockbackForce;
    private Vector2 knockbackDirection;

    public void Initialize(int dmg, float force, Vector2 direction)
    {
        damage = dmg;
        knockbackForce = force;
        knockbackDirection = direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PauseMenu.GameIsPaused) return;

        if (collision.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
            
            // Check if the object also has an EnemyHealth component for knockback
            if (collision.TryGetComponent(out EnemyHealth enemy))
            {
                enemy.ApplyKnockback(knockbackDirection, knockbackForce);
            }
            
            Debug.Log($"Damaged {collision.name} for {damage} damage");
        }
    }
}
