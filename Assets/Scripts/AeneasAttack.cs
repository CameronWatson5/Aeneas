using UnityEngine;
using System.Collections;

public class AeneasAttack : MonoBehaviour
{
    [System.Serializable]
    public class SwishVisual
    {
        public GameObject prefab;
        public Vector2 offset;
        public Vector2 scale = Vector2.one;
    }

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float knockbackForce = 10f;

    [Header("Swish Visuals")]
    [SerializeField] private SwishVisual swishLeft;
    [SerializeField] private SwishVisual swishRight;
    [SerializeField] private SwishVisual swishUp;
    [SerializeField] private SwishVisual swishDown;
    [SerializeField] private float swishDuration = 0.5f;

    [Header("Audio")]
    [SerializeField] private AudioClip attackSound;

    private float lastAttackTime;
    private Animator animator;
    private PlayerMovement playerMovement;
    private AudioSource audioSource;

    private void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();

        if (!animator || !playerMovement || !audioSource)
        {
            Debug.LogError("Required components missing on the player!");
            enabled = false;
        }
    }

    public void TriggerAttack()
    {
        if (CanAttack())
        {
            Attack();
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
        Vector2 attackDirection = DetermineAttackDirection();
        string attackTrigger = GetAttackTrigger(attackDirection);
        animator.SetTrigger(attackTrigger);

        PlaySwishAnimation(attackDirection);
        PlayAttackSound();

        Debug.Log($"Attack performed: Direction={attackTrigger}");
    }

    private Vector2 DetermineAttackDirection()
    {
        Vector2 lastDirection = playerMovement.GetLastMovementDirection();
        return Vector2.ClampMagnitude(lastDirection, 1f);
    }

    private string GetAttackTrigger(Vector2 direction)
    {
        return Mathf.Abs(direction.x) > Mathf.Abs(direction.y) 
            ? (direction.x > 0 ? "AttackRight" : "AttackLeft")
            : (direction.y > 0 ? "AttackUp" : "AttackDown");
    }

    private void PlaySwishAnimation(Vector2 attackDirection)
    {
        SwishVisual swishVisual = GetSwishVisual(attackDirection);

        if (swishVisual?.prefab != null)
        {
            Vector2 swishPosition = (Vector2)transform.position + swishVisual.offset;
            GameObject swishInstance = Instantiate(swishVisual.prefab, swishPosition, Quaternion.identity, transform);

            swishInstance.transform.localScale = new Vector3(swishVisual.scale.x, swishVisual.scale.y, 1f);

            SetupSwishCollider(swishInstance, attackDirection);

            Animator swishAnimator = swishInstance.GetComponent<Animator>();
            swishAnimator?.SetTrigger("Swish");

            Destroy(swishInstance, swishDuration);

            Debug.Log($"Swish animation: Position={swishPosition}, Scale={swishVisual.scale}, Direction={attackDirection}");
        }
    }

    private void SetupSwishCollider(GameObject swishInstance, Vector2 attackDirection)
    {
        BoxCollider2D swishCollider = swishInstance.AddComponent<BoxCollider2D>();
        swishCollider.isTrigger = true;

        SwishDamageDealer damageDealer = swishInstance.AddComponent<SwishDamageDealer>();
        damageDealer.Initialize(attackDamage, knockbackForce, attackDirection);
    }

    private SwishVisual GetSwishVisual(Vector2 attackDirection)
    {
        return Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y)
            ? (attackDirection.x > 0 ? swishRight : swishLeft)
            : (attackDirection.y > 0 ? swishUp : swishDown);
    }

    private void PlayAttackSound()
    {
        if (attackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }
}

