using UnityEngine;

public class PopupTrigger : MonoBehaviour
{
    public string popupText;
    public float popupDelay = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PopUp popup = FindObjectOfType<PopUp>();
            if (popup != null)
            {
                StartCoroutine(popup.ShowPopupWithDelay(popupDelay, popupText));
            }
        }
    }
}