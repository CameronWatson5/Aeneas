using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;

    [Header("Interaction UI")]
    public GameObject interactionPrompt;
    public TextMeshProUGUI interactionText;
    public Button interactButton;

    [Header("Control UI")]
    public Button pauseButton;
    public Button attackButton;
    public JoystickController joystick;

    private IInteractable currentInteractable;

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
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(PauseGame);
        }

        if (interactButton != null)
        {
            interactButton.onClick.AddListener(OnInteractButtonPressed);
            interactButton.gameObject.SetActive(false);
        }

        if (attackButton != null)
        {
            attackButton.onClick.AddListener(OnAttackButtonPressed);
        }

        if (joystick == null)
        {
            Debug.LogWarning("Joystick not assigned in UIManager");
        }
    }

    private void OnAttackButtonPressed()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            AeneasAttack aeneasAttack = player.GetComponent<AeneasAttack>();
            if (aeneasAttack != null)
            {
                aeneasAttack.TriggerAttack();
            }
        }
    }

    public void ShowInteractionPrompt(string message, IInteractable interactable)
    {
        if (interactionText != null)
        {
            interactionText.text = message;
        }
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(true);
        }
        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(true);
        }
        currentInteractable = interactable;
    }

    public void HideInteractionPrompt()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(false);
        }
        currentInteractable = null;
    }

    private void OnInteractButtonPressed()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    public void PauseGame()
    {
        if (PauseMenu.Instance != null)
        {
            PauseMenu.Instance.Pause();
        }
        else
        {
            Debug.LogError("PauseMenu instance not found!");
        }
    }

    public Vector2 GetJoystickInput()
    {
        return joystick != null ? joystick.inputVector : Vector2.zero;
    }
}
