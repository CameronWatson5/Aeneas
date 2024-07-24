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