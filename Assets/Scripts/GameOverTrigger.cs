using System.Collections;
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
                Debug.Log("GameOverTrigger: Correct mission, showing normal popup and applying damage");
                PopUpManager.Instance.ShowPopup("Game Over", "You have been defeated by the enemies.");
                
                // Damage the player significantly after a short delay
                StartCoroutine(ApplyDamageToPlayer());
            }
        }
    }

    private IEnumerator ApplyDamageToPlayer()
    {
        yield return new WaitForSeconds(2); // Adjust the delay as needed
        AeneasAttributes playerAttributes = FindObjectOfType<AeneasAttributes>();
        if (playerAttributes != null)
        {
            playerAttributes.TakeDamage(1000); // Apply significant damage to trigger GameOver
        }
    }
}