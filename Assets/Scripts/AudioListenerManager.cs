// This script is used to keep track of the Audio Listener Objects in the game.
// This script also deletes duplicates  
using UnityEngine;

public class AudioListenerManager : MonoBehaviour
{
    private static AudioListenerManager _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            AudioListener audioListener = GetComponent<AudioListener>();
            if (audioListener != null)
            {
                Destroy(audioListener);
            }
        }
    }
}