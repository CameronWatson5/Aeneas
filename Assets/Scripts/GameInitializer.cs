using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour
{
    public string persistentManagersSceneName = "PersistentManagers";

    void Start()
    {
        if (!SceneManager.GetSceneByName(persistentManagersSceneName).isLoaded)
        {
            SceneManager.LoadScene(persistentManagersSceneName, LoadSceneMode.Additive);
        }
    }
}