using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public Image mapImage;
    public GameObject playerIconPrefab;
    private GameObject playerIcon;

    public void SetupMap(Vector2 mapSize, Vector2 worldSize, Sprite mapSprite)
    {
        mapImage.sprite = mapSprite;
        mapImage.rectTransform.sizeDelta = mapSize;

        // Calculate and set player icon position
        if (playerIcon == null)
        {
            playerIcon = Instantiate(playerIconPrefab, mapImage.transform);
        }
        UpdatePlayerIconPosition(worldSize);
    }

    public void SetupMapForScene(string sceneName)
    {
        // Load the appropriate map sprite and world size for each scene
        Sprite mapSprite = LoadMapSpriteForScene(sceneName);
        Vector2 worldSize = GetWorldSizeForScene(sceneName);

        if (mapSprite == null || worldSize == Vector2.zero)
        {
            // Default to Troy map if no specific map is found
            mapSprite = Resources.Load<Sprite>("Art/Maps/TroyMap");
            worldSize = new Vector2(100, 107);
        }

        Vector2 mapSize = new Vector2(300, 300); // Example size, adjust as needed
        SetupMap(mapSize, worldSize, mapSprite);
    }

    Sprite LoadMapSpriteForScene(string sceneName)
    {
        // Load and return the appropriate map sprite based on the scene name
        if (sceneName == "Troy" || sceneName == "Indoor")
        {
            return Resources.Load<Sprite>("Art/Maps/TroyMap");
        }
        return null;
    }

    Vector2 GetWorldSizeForScene(string sceneName)
    {
        // Return the world size for the given scene name
        if (sceneName == "Troy" || sceneName == "Indoor")
        {
            return new Vector2(100, 107); 
        }
        
        return Vector2.zero;
    }

    void UpdatePlayerIconPosition(Vector2 worldSize)
    {
        Vector3 playerPos = new Vector3(
            PlayerPrefs.GetFloat("PlayerPosX", 0),
            PlayerPrefs.GetFloat("PlayerPosY", 0),
            PlayerPrefs.GetFloat("PlayerPosZ", 0)
        );

        RectTransform mapRect = mapImage.rectTransform;

        float normalizedX = (playerPos.x + worldSize.x / 2) / worldSize.x;
        float normalizedY = (playerPos.y + worldSize.y / 2) / worldSize.y;

        // Adjust the position by subtracting from both x and y for the Troy scene
        Vector2 mapIconPos = new Vector2(
            (normalizedX * mapRect.sizeDelta.x),
            (normalizedY * mapRect.sizeDelta.y)
        );

        if (PlayerPrefs.GetString("PreviousScene", "Troy") == "Troy")
        {
            mapIconPos -= new Vector2(100, 100);
        }

        playerIcon.GetComponent<RectTransform>().anchoredPosition = mapIconPos;
    }

    void Update()
    {
        if (playerIcon != null)
        {
            Vector2 worldSize = GetWorldSizeForScene(PlayerPrefs.GetString("PreviousScene", "Troy"));
            UpdatePlayerIconPosition(worldSize);
        }
    }
}
