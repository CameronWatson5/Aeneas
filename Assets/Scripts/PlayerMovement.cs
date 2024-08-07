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

    [Header("Touch Controls")]
    private JoystickController joystick;
    private bool joystickInitialized = false;
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
        InitializeJoystick();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}");
        InitializeJoystick();
        if (scene.name == "Troy")
        {
            PositionPlayer();
        }
    }

    private void InitializeJoystick()
    {
        if (UIManager.Instance != null)
        {
            joystick = UIManager.Instance.joystick;
            joystickInitialized = (joystick != null);
            Debug.Log(joystickInitialized ? "Joystick initialized successfully." : "Joystick not found in UIManager");
        }
        else
        {
            Debug.LogWarning("UIManager instance not found");
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

        // Keyboard input
        float keyboardHorizontal = Input.GetAxisRaw(HORIZONTAL);
        float keyboardVertical = Input.GetAxisRaw(VERTICAL);

        // Joystick input
        Vector2 joystickInput = Vector2.zero;
        if (joystickInitialized)
        {
            joystickInput = joystick.inputVector;
        }

        // Combine inputs, prioritizing keyboard if both are used
        change.x = keyboardHorizontal != 0 ? keyboardHorizontal : joystickInput.x;
        change.y = keyboardVertical != 0 ? keyboardVertical : joystickInput.y;

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
