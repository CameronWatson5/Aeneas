// This is used in the pause menu to show the equip able items.

using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using UnityEngine.Events;

public class EquippableItemUI : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI priceText;
    public Button buyButton;

    private ShopItem item;

    public void SetupItem(ShopItem shopItem, UnityAction<ShopItem> buyCallback)
    {
        item = shopItem;
        if (itemIcon != null && item.icon != null)
            itemIcon.sprite = item.icon;
        if (itemNameText != null)
            itemNameText.text = item.itemName;
        if (priceText != null)
            priceText.text = item.price.ToString() + " Gold";

        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(() => buyCallback(item));
        }
        
        Debug.Log($"Set up item: {item.itemName} with price {item.price}");
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
}