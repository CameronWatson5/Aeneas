// This script is used for the joystick on the bottom left of the screen.
// It can be used with touch controls or the mouse.

using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform background;
    public RectTransform handle;
    [HideInInspector]
    public Vector2 inputVector;

    private float joystickRange;

    void Start()
    {
        joystickRange = background.rect.width * 0.5f;
        ResetHandle();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background, eventData.position, eventData.pressEventCamera, out position))
        {
            position = Vector2.ClampMagnitude(position, joystickRange);
            handle.anchoredPosition = position;

            float magnitude = position.magnitude / joystickRange;
            inputVector = position.normalized * magnitude;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ResetHandle();
    }

    private void ResetHandle()
    {
        handle.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;
    }
}