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

    [Header("Stats and Mission UI")]
    public GameObject statsPanel;
    public GameObject missionPanel;

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
        SetupButtons();
        SetupJoystick();
        PositionPanels();
    }

    private void SetupButtons()
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
    }

    private void SetupJoystick()
    {
        if (joystick == null)
        {
            Debug.LogWarning("Joystick not assigned in UIManager");
        }
        else
        {
            Debug.Log("Joystick assigned in UIManager");
        }
    }

    private void PositionPanels()
    {
        PositionPanel(statsPanel, 0.1f, true);  // Position at top 10%
        PositionPanel(missionPanel, 0.1f, false);  // Position at bottom 10%
    }

    private void PositionPanel(GameObject panel, float heightPercentage, bool isTop)
    {
        if (panel != null)
        {
            RectTransform rectTransform = panel.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchorMin = new Vector2(0, isTop ? 1 - heightPercentage : 0);
                rectTransform.anchorMax = new Vector2(1, isTop ? 1 : heightPercentage);
                rectTransform.anchoredPosition = Vector2.zero;
                rectTransform.sizeDelta = Vector2.zero;
            }
            panel.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"{(isTop ? "Stats" : "Mission")} panel is not assigned in UIManager");
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