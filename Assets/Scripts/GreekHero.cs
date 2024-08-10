using UnityEngine;

public class GreekHero : MonoBehaviour
{
    public string heroName;          // The unique name of the Greek Hero
    public string popupMessage;      // Message to display when the hero is defeated

    private EnemyHealth enemyHealth; // Reference to the EnemyHealth component

    private void Start()
    {
        // Get the EnemyHealth component attached to this game object
        enemyHealth = GetComponent<EnemyHealth>();
        if (enemyHealth == null)
        {
            Debug.LogError("GreekHero: EnemyHealth component not found!");
            return;
        }

        // Subscribe to the OnEnemyDeath event to handle when this hero is defeated
        enemyHealth.OnEnemyDeath += OnDefeated;

        // Check if this hero has already been defeated (status saved in MissionManager)
        if (MissionManager.Instance.missionTargets.ContainsKey(heroName) && MissionManager.Instance.missionTargets[heroName])
        {
            // If already defeated, destroy this hero game object
            Destroy(gameObject);
        }
    }

    private void OnDefeated(GameObject defeatedHero)
    {
        // If the defeated hero is this game object
        if (defeatedHero == this.gameObject)
        {
            // Mark this hero as defeated in the MissionManager
            MissionManager.Instance.SetMissionTargetStatus(heroName, true);

            // Display a pop-up message for this hero's defeat using the PopUpManager
            if (PopUpManager.Instance != null)
            {
                PopUpManager.Instance.ShowPopup(popupMessage, heroName);
            }
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the OnEnemyDeath event when this object is destroyed
        if (enemyHealth != null)
        {
            enemyHealth.OnEnemyDeath -= OnDefeated;
        }
    }
}