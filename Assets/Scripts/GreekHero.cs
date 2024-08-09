using UnityEngine;

public class GreekHero : MonoBehaviour
{
    public string heroName;
    public string popupMessage; 
    private EnemyHealth enemyHealth;

    private void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        if (enemyHealth == null)
        {
            Debug.LogError("GreekHero: EnemyHealth component not found!");
            return;
        }

        enemyHealth.OnEnemyDeath += OnDefeated;

        if (MissionManager.Instance.missionTargets.ContainsKey(heroName) && MissionManager.Instance.missionTargets[heroName])
        {
            Destroy(gameObject); 
        }
    }

    private void OnDefeated(GameObject defeatedHero)
    {
        if (defeatedHero == this.gameObject)
        {
            MissionManager.Instance.SetMissionTargetStatus(heroName, true);

            if (PopUpManager.Instance != null)
            {
                PopUpManager.Instance.ShowPopup(popupMessage, heroName);
            }
        }
    }

    private void OnDestroy()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnEnemyDeath -= OnDefeated;
        }
    }
}