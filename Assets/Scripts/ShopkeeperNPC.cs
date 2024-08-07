using UnityEngine;
using UnityEngine.UI;

public class ShopkeeperNPC : MonoBehaviour, IInteractable
{
    private bool playerNearby = false;
    private ShopManager shopManager;
    public Button interactButton; // Reference to the UI button for interaction

    void Start()
    {
        shopManager = FindObjectOfType<ShopManager>();
        if (shopManager == null)
        {
            Debug.LogError("ShopManager not found in the scene!");
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log("Player entered shop area");
            UIManager.Instance.ShowInteractionPrompt("Press E to Interact", this);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            Debug.Log("Player left shop area");
            UIManager.Instance.HideInteractionPrompt();
        }
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public void Interact()
    {
        if (playerNearby)
        {
            ToggleShop();
        }
    }

    private void ToggleShop()
    {
        if (shopManager != null)
        {
            shopManager.ToggleShop();
            Debug.Log("Attempting to toggle shop");
        }
        else
        {
            Debug.LogError("ShopManager is null!");
        }
    }
    private void SetupInteractButton()
    {
        if (interactButton != null)
        {
            interactButton.onClick.AddListener(OnInteractButtonPressed);
            interactButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Interact button not assigned in the inspector for ShopkeeperNPC.");
        }
    }

   

    private void OnInteractButtonPressed()
    {
        if (playerNearby)
        {
            ToggleShop();
        }
    }
}