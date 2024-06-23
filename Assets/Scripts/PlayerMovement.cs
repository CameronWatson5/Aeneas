using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    
    private Rigidbody2D myRigidBody;
    private Vector2 change;
    private Animator animator;
    
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private const string MOVE_X = "moveX";
    private const string MOVE_Y = "moveY";
    private const string MOVING = "moving";

    private void Start()
    {
        animator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // GetAxisRaw is used because it is more responsive than GetAxis.
        // This allows for quick movement and instant movement.
        change.x = Input.GetAxisRaw(HORIZONTAL);
        change.y = Input.GetAxisRaw(VERTICAL);

        // Normalize for consistent diagonal speed
        if (change.magnitude > 1)
        {
            change.Normalize();
        }

        UpdateAnimationAndMove();
    }

    private void UpdateAnimationAndMove()
    {
        if (change != Vector2.zero)
        {
            MoveCharacter();
            animator.SetFloat(MOVE_X, change.x);
            animator.SetFloat(MOVE_Y, change.y);
            animator.SetBool(MOVING, true);
        }
        else
        {
            animator.SetBool(MOVING, false);
        }
    }

    private void MoveCharacter()
    {
        Vector2 targetPosition = myRigidBody.position + change * speed * Time.fixedDeltaTime;
        myRigidBody.MovePosition(Vector2.MoveTowards(myRigidBody.position, targetPosition, speed * Time.fixedDeltaTime));
    }
}