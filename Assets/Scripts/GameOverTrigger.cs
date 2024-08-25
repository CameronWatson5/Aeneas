// Script is used to transition the player to the game over screen.

using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    private bool isTriggered = false;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTriggered && collision.CompareTag("Player"))
        {
            isTriggered = true;
            Debug.Log("GameOverTrigger: Player entered trigger");

            if (MissionManager.Instance.IsCurrentMission(3))
            {
                Debug.Log("GameOverTrigger: Correct mission, transitioning to GameOver scene");
                TransitionToGameOver();
            }
        }
    }

    private void TransitionToGameOver()
    {
        // Show popup if needed
        if (PopUpManager.Instance != null)
        {
            PopUpManager.Instance.ShowPopup("Game Over", "You have been defeated by the enemies.");
        }

        // Transition to GameOver scene
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.TransitionToScene("GameOver", "");
        }
        else
        {
            Debug.LogError("GameOverTrigger: SceneTransitionManager not found");
        }
    }
}