using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUI : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text itemNameText;
    public Button itemButton;

    private InventoryItem item;
    private System.Action<InventoryItem> onClickCallback;

    public void SetupItem(InventoryItem inventoryItem, System.Action<InventoryItem> callback)
    {
        item = inventoryItem;
        Debug.Log($"Setting up item: {item.itemName}, Icon: {item.icon}");

        if (iconImage != null && item.icon != null)
            iconImage.sprite = item.icon;
        if (itemNameText != null)
            itemNameText.text = item.itemName;
        onClickCallback = callback;

        if (itemButton != null)
            itemButton.onClick.AddListener(() => onClickCallback(item));
    }
}