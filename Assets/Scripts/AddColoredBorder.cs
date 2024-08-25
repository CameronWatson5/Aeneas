// Unused script

using UnityEngine;
using UnityEngine.UI;

public class AddColoredBorder : MonoBehaviour
{
    public Color panelColor = Color.white;
    public Color borderColor = Color.black;
    public float borderWidth = 10f;

    void Start()
    {
        // Create the panel
        GameObject panel = new GameObject("Panel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        panel.transform.SetParent(transform);
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(300, 200); 
        Image panelImage = panel.GetComponent<Image>();
        panelImage.color = panelColor;

        // Create the border
        GameObject border = new GameObject("Border", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        border.transform.SetParent(panel.transform.parent); // Set border as sibling to panel
        RectTransform borderRect = border.GetComponent<RectTransform>();
        borderRect.sizeDelta = new Vector2(panelRect.sizeDelta.x + 2 * borderWidth,
            panelRect.sizeDelta.y + 2 * borderWidth);
        borderRect.anchoredPosition = panelRect.anchoredPosition;
        Image borderImage = border.GetComponent<Image>();
        borderImage.color = borderColor;

        // Move border behind the panel
        border.transform.SetSiblingIndex(panel.transform.GetSiblingIndex() - 1);
    }
}