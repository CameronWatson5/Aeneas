// using UnityEngine;
//
// public class CutsceneController : MonoBehaviour
// {
//     public GameObject cutscenePrefab;
//
//     public void ShowCutscene()
//     {
//         if (cutscaenePrefab == null)
//         {
//             Debug.LogError("CutsceneController: cutscenePrefab is not assigned.");
//             return;
//         }
//
//         GameObject cutsceneObject = Instantiate(cutscenePrefab);
//         IndoorCutsceneManager cutsceneManager = cutsceneObject.GetComponent<IndoorCutsceneManager>();
//         if (cutsceneManager != null)
//         {
//             cutsceneManager.TriggerCutsceneWithDelay(0.5f);
//         }
//         else
//         {
//             Debug.LogError("IndoorCutsceneManager not found on instantiated prefab.");
//         }
//     }
// }