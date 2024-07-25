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
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            Debug.Log("Player left shop area");
        }
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.R))
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