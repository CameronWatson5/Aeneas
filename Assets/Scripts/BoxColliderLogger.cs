// This script was used for testing

using UnityEngine;

public class BoxColliderLogger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Object entered trigger: " + other.gameObject.name);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Object staying in trigger: " + other.gameObject.name);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Object exited trigger: " + other.gameObject.name);
    }
}