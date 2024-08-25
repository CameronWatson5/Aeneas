// This script is used to keep track of the event system and destroy duplicates.

using UnityEngine;
using UnityEngine.EventSystems;

public class SingletonEventSystem : MonoBehaviour
{
    private static EventSystem _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = GetComponent<EventSystem>();
            DontDestroyOnLoad(gameObject);
            Debug.Log("SingletonEventSystem created: " + gameObject.name);
        }
        else
        {
            Debug.Log("Destroying duplicate EventSystem: " + gameObject.name);
            Destroy(gameObject);
        }
    }
}