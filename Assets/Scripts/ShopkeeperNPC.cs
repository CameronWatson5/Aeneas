using UnityEngine;

public class ShopkeeperNPC : MonoBehaviour
{
    private bool playerNearby = false;
    private ShopManager shopManager;

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
            UIManager.Instance.ShowInteractionPrompt("Press E to interact");
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
    }
}