// This script is used for controls

using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public Button interactButton;

    private bool isInteractPressed = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (interactButton != null)
        {
            interactButton.onClick.AddListener(OnInteractButtonPressed);
        }
    }

    private void Update()
    {
        // Check for keyboard input
        if (Input.GetKeyDown(KeyCode.E))
        {
            isInteractPressed = true;
        }
    }

    private void LateUpdate()
    {
        // Reset the interact flag at the end of each frame
        isInteractPressed = false;
    }

    public bool IsInteractPressed()
    {
        return isInteractPressed;
    }

    private void OnInteractButtonPressed()
    {
        isInteractPressed = true;
    }
}