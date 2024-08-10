using UnityEngine;

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
    private Animator playerAnimator;
    private Animator swishAnimator; 
    private PlayerMovement playerMovement;
    private AudioSource audioSource;
    private AeneasAttributes aeneasAttributes; 

    private void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        // Get the main Animator for player animations
        playerAnimator = GetComponent<Animator>();

        // Find and get the secondary Animator for swish animations 
        Transform swishAnimatorTransform = transform.Find("SwishAnimator");
        if (swishAnimatorTransform != null)
        {
            swishAnimator = swishAnimatorTransform.GetComponent<Animator>();
            if (swishAnimator == null)
            {
                Debug.LogError("SwishAnimator component is missing on the child GameObject!");
                enabled = false;
                return;
            }
        }
        else
        {
            Debug.LogError("SwishAnimator child not found!");
            enabled = false;
            return;
        }

        playerMovement = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
        aeneasAttributes = GetComponent<AeneasAttributes>(); // Initialize AeneasAttributes reference

        if (!playerAnimator || !playerMovement || !audioSource || !aeneasAttributes)
        {
            Debug.LogError("Required components missing on the player!");
            enabled = false;
            return;
        }

        Debug.Log("Components initialized successfully.");
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
        if (PauseMenu.GameIsPaused || PopUp.IsPopupActive || SpecialPopUp.IsSpecialPopupActive) return; // Prevent attack if the game is paused or a popup is active

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
        playerAnimator.SetTrigger(attackTrigger); // Trigger the attack animation on the player

        PlaySwishAnimation(attackDirection);
        PlayAttackSound();

        Debug.Log($"Attack performed: Direction={attackTrigger}");
    }

    private Vector2 DetermineAttackDirection()
    {
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement is null.");
            return Vector2.down; // Default to down if playerMovement is null
        }

        Vector2 lastDirection = playerMovement.GetLastMovementDirection();
        if (lastDirection == Vector2.zero)
        {
            lastDirection = Vector2.down; // Default to down if the direction is zero
        }

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

            // Trigger the swish animation on the swishAnimator
            swishAnimator.SetTrigger(GetSwishTrigger(attackDirection));

            Destroy(swishInstance, swishDuration);

            Debug.Log($"Swish animation: Position={swishPosition}, Scale={swishVisual.scale}, Direction={attackDirection}");
        }
    }

    private string GetSwishTrigger(Vector2 direction)
    {
        return Mathf.Abs(direction.x) > Mathf.Abs(direction.y)
            ? (direction.x > 0 ? "SwishRight" : "SwishLeft")
            : (direction.y > 0 ? "SwishUp" : "SwishDown");
    }

    private void SetupSwishCollider(GameObject swishInstance, Vector2 attackDirection)
    {
        BoxCollider2D swishCollider = swishInstance.AddComponent<BoxCollider2D>();
        swishCollider.isTrigger = true;

        SwishDamageDealer damageDealer = swishInstance.AddComponent<SwishDamageDealer>();
        damageDealer.Initialize(aeneasAttributes.damage, knockbackForce, attackDirection); // Use damage from AeneasAttributes
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
