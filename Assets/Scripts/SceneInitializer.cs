using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    public GameObject indoorCutsceneManagerPrefab;

    void Awake()
    {
        if (IndoorCutsceneManager.Instance == null)
        {
            var instance = Instantiate(indoorCutsceneManagerPrefab);
            Debug.Log("SceneInitializer: IndoorCutsceneManager instance instantiated.");
            if (!instance.activeSelf)
            {
                instance.SetActive(true);
                Debug.Log("SceneInitializer: IndoorCutsceneManager instance set to active.");
            }
        }
    }
}