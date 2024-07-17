// This is the player movement script.
// This is used so that the player (Aeneas) can move.
// Furthermore, it triggers the animation.
// The default speed is 5, however, this can be changed in Unity.

using UnityEngine;
<<<<<<< HEAD
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(AeneasAttributes))]
public class PlayerMovement : MonoBehaviour
{
    private static PlayerMovement instance;
    private Rigidbody2D myRigidBody;
    private Vector2 change;
    private Animator animator;
    private AeneasAttributes aeneasAttributes;
=======

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    
    private Rigidbody2D myRigidBody;
    private Vector2 change;
    private Animator animator;
>>>>>>> gitlab/main
    
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private const string MOVE_X = "moveX";
    private const string MOVE_Y = "moveY";
    private const string MOVING = "moving";

<<<<<<< HEAD
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

=======
>>>>>>> gitlab/main
    private void Start()
    {
        animator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
<<<<<<< HEAD
        aeneasAttributes = GetComponent<AeneasAttributes>();
        if (aeneasAttributes == null)
        {
            Debug.LogError("AeneasAttributes component not found on the player object!");
        }
        PositionPlayer();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PositionPlayer();
    }

    private void PositionPlayer()
    {
        if (PlayerPrefs.HasKey("SpawnPointIdentifier"))
        {
            string spawnPointId = PlayerPrefs.GetString("SpawnPointIdentifier");
            GameObject spawnPoint = GameObject.Find(spawnPointId);
            if (spawnPoint != null)
            {
                transform.position = spawnPoint.transform.position;
            }
            else
            {
                Debug.LogWarning($"Spawn point '{spawnPointId}' not found in the scene.");
            }
            PlayerPrefs.DeleteKey("SpawnPointIdentifier");
        }
        else if (PlayerPrefs.HasKey("SpawnPositionX") && PlayerPrefs.HasKey("SpawnPositionY"))
        {
            float x = PlayerPrefs.GetFloat("SpawnPositionX");
            float y = PlayerPrefs.GetFloat("SpawnPositionY");
            transform.position = new Vector2(x, y);
        
            PlayerPrefs.DeleteKey("SpawnPositionX");
            PlayerPrefs.DeleteKey("SpawnPositionY");
        }
=======
>>>>>>> gitlab/main
    }

    private void FixedUpdate()
    {
<<<<<<< HEAD
        change.x = Input.GetAxisRaw(HORIZONTAL);
        change.y = Input.GetAxisRaw(VERTICAL);

        if (change.sqrMagnitude > 1)
=======
        // GetAxisRaw is used because it is more responsive than GetAxis.
        // This allows for quick movement and instant movement.
        change.x = Input.GetAxisRaw(HORIZONTAL);
        change.y = Input.GetAxisRaw(VERTICAL);

        // Normalize for consistent diagonal speed
        if (change.magnitude > 1)
>>>>>>> gitlab/main
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
<<<<<<< HEAD
        float currentSpeed = aeneasAttributes != null ? aeneasAttributes.speed : 5f;
        Vector2 targetPosition = myRigidBody.position + change * currentSpeed * Time.fixedDeltaTime;
        myRigidBody.MovePosition(targetPosition);
    }

    public Vector2 GetLastMovementDirection()
    {
        return new Vector2(animator.GetFloat(MOVE_X), animator.GetFloat(MOVE_Y)).normalized;
    }

    public Vector2 GetMovementInput()
    {
        return change;
=======
        Vector2 targetPosition = myRigidBody.position + change * speed * Time.fixedDeltaTime;
        myRigidBody.MovePosition(Vector2.MoveTowards(myRigidBody.position, targetPosition, speed * Time.fixedDeltaTime));
>>>>>>> gitlab/main
    }
}