using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(AeneasAttributes))]
public class PlayerMovement : MonoBehaviour
{
    private static PlayerMovement instance;
    private Rigidbody2D myRigidBody;
    private Vector2 change;
    private Vector2 lastMovementDirection;
    private Animator animator;
    private AeneasAttributes aeneasAttributes;
    private bool canMove = true;

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private const string MOVE_X = "moveX";
    private const string MOVE_Y = "moveY";
    private const string MOVING = "moving";

    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        aeneasAttributes = GetComponent<AeneasAttributes>();
        if (aeneasAttributes == null)
        {
            Debug.LogError("AeneasAttributes component not found on the player object!");
        }
        PositionPlayer();
        ResetPlayerVelocity();
        change = Vector2.zero;
        lastMovementDirection = Vector2.zero;
        Debug.Log("PlayerMovement initialized.");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Troy")
        {
            PositionPlayer();
        }
    }

    private void PositionPlayer()
    {
        string spawnPointId = PlayerPrefs.GetString("SpawnPointIdentifier", "DefaultSpawnPoint");
        GameObject spawnPoint = GameObject.Find(spawnPointId);
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.transform.position;
            ResetPlayerVelocity();
        }
        else
        {
            Debug.LogWarning($"Spawn point '{spawnPointId}' not found in the scene.");
        }
        PlayerPrefs.DeleteKey("SpawnPointIdentifier");
    }

    private void ResetPlayerVelocity()
    {
        if (myRigidBody != null)
        {
            myRigidBody.velocity = Vector2.zero;
            myRigidBody.angularVelocity = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            animator.SetBool(MOVING, false);
            return;
        }

        change.x = Input.GetAxisRaw(HORIZONTAL);
        change.y = Input.GetAxisRaw(VERTICAL);

        if (change.sqrMagnitude > 1)
        {
            change.Normalize();
        }

        UpdateAnimationAndMove();
    }

    private void UpdateAnimationAndMove()
    {
        if (change != Vector2.zero)
        {
            lastMovementDirection = change;
            MoveCharacter();
            animator.SetFloat(MOVE_X, change.x);
            animator.SetFloat(MOVE_Y, change.y);
            animator.SetBool(MOVING, true);
        }
        else
        {
            animator.SetBool(MOVING, false);
        }

        PlayerPrefs.SetFloat("PlayerPosX", transform.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", transform.position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", transform.position.z);
    }

    private void MoveCharacter()
    {
        float currentSpeed = aeneasAttributes != null ? aeneasAttributes.speed : 5f;
        Vector2 targetPosition = myRigidBody.position + change * currentSpeed * Time.fixedDeltaTime;
        myRigidBody.MovePosition(targetPosition);
    }

    public Vector2 GetLastMovementDirection()
    {
        return lastMovementDirection.normalized;
    }

    public Vector2 GetMovementInput()
    {
        return change;
    }
}
