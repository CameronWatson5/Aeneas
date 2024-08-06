using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public Image mapImage; 
    public GameObject playerIconPrefab;
    private GameObject playerIcon;

    [Header("Zoom Settings")]
    public float zoomSpeed = 0.1f;
    public float minZoom = 0.5f;
    public float maxZoom = 2f;
    private float currentZoom = 1f;

    [Header("Pan Settings")]
    public float panSpeed = 0.5f;
    private Vector2 lastPanPosition;
    private bool isPanning;

    private Vector2 currentUVOffset = Vector2.zero;

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
        // Load the appropriate map texture and world size for each scene
        Sprite mapSprite = LoadMapSpriteForScene(sceneName);
        Vector2 worldSize = GetWorldSizeForScene(sceneName);

        if (mapSprite == null || worldSize == Vector2.zero)
        {
            // Default to Troy map if no specific map is found
            mapSprite = Resources.Load<Sprite>("Art/Maps/TroyMap");
            worldSize = new Vector2(299, 306);
        }

        Vector2 mapSize = new Vector2(300, 300);
        SetupMap(mapSize, worldSize, mapSprite);
    }

    Sprite LoadMapSpriteForScene(string sceneName)
    {
        // Load and return the appropriate map texture based on the scene name
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
            return new Vector2(299, 306);
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
            mapIconPos -= new Vector2(50, 100);
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

        HandleZoom();
        HandlePan();
    }

    void HandleZoom()
    {
        float scrollData;
        if (Input.touchSupported && Input.touchCount == 2)
        {
            // Handle pinch zoom for touch devices
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            scrollData = prevTouchDeltaMag - touchDeltaMag;
        }
        else
        {
            // Handle zoom for mouse wheel
            scrollData = Input.GetAxis("Mouse ScrollWheel");
        }

        if (scrollData != 0.0f)
        {
            currentZoom = Mathf.Clamp(currentZoom - scrollData * zoomSpeed, minZoom, maxZoom);
            mapImage.rectTransform.localScale = new Vector3(currentZoom, currentZoom, 1);
        }
    }

    void HandlePan()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isPanning = true;
            lastPanPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isPanning = false;
        }

        if (isPanning)
        {
            Vector2 delta = (Vector2)Input.mousePosition - lastPanPosition;
            mapImage.rectTransform.anchoredPosition += delta * panSpeed / currentZoom;
            lastPanPosition = Input.mousePosition;
        }
    }
}
