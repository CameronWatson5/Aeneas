using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AeneasAttack : MonoBehaviour
{
    public Animator animator;
    public float attackCooldown = 0.5f;
    [SerializeField] private float attackRange = 1f;

    private float lastAttackTime;
    private AeneasAttributes attributes;
    private PlayerMovement playerMovement;

    private void Start()
    {
        attributes = GetComponent<AeneasAttributes>();
        playerMovement = GetComponent<PlayerMovement>();
        
        if (attributes == null)
        {
            Debug.LogError("AeneasAttributes component not found on the player!");
        }
        
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement component not found on the player!");
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator component not found on the player!");
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CanAttack())
        {
            Attack();
        }
    }

    private bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown;
    }

    private void Attack()
    {
        lastAttackTime = Time.time;
        
        string attackTrigger = DetermineAttackDirection();

        animator.SetTrigger(attackTrigger);

        PerformAttack();
    }

    private string DetermineAttackDirection()
    {
        Vector2 movement = playerMovement.GetLastMovementDirection();

        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        {
            return (movement.x > 0) ? "AttackRight" : "AttackLeft";
        }
        else if (movement.y != 0)
        {
            return (movement.y > 0) ? "AttackUp" : "AttackDown";
        }

        return "AttackDown"; // Default
    }

    private void PerformAttack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

        bool hitEnemy = false;
        foreach (Collider2D collider in hitColliders)
        {
            EnemyHealth enemy = collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(attributes.damage);
                hitEnemy = true;
            }
        }

        if (hitEnemy)
        {
            // Play hit sound or particle effect
            // AudioManager.Instance.PlaySound("EnemyHit");
            // ParticleSystem.Play();
        }
    }

    public void TriggerAttack()
    {
        if (CanAttack())
        {
            Attack();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}