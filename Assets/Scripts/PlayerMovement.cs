// This is the player movement script.
// This is used so that the player (Aeneas) can move.
// Furthermore, it triggers the animation.
// The default speed is 5, however, this can be changed in Unity.

using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    
    private static PlayerMovement instance;
    private Rigidbody2D myRigidBody;
    private Vector2 change;
    private Animator animator;
    
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private const string MOVE_X = "moveX";
    private const string MOVE_Y = "moveY";
    private const string MOVING = "moving";

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
        // If neither spawn point nor coordinates are set, the player will remain at their current position
    }

    private void FixedUpdate()
    {
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
        myRigidBody.MovePosition(targetPosition);
    }
}