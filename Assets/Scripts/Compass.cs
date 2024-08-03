using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Compass : MonoBehaviour
{
    public Image arrowImage;
    public Transform player;
    public string troySceneName = "Troy"; 
    private Transform target;
    private Transform[] greekHeroes;
    private Transform nearestHero;

    private bool missionComplete;

    private void Start()
    {
        FindGreekHeroes();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (IsInTroyScene())
        {
            if (missionComplete)
            {
                if (target != null)
                {
                    Vector3 direction = target.position - player.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    arrowImage.rectTransform.localRotation = Quaternion.Euler(0, 0, angle - 90); // Adjust for arrow orientation
                    arrowImage.enabled = true;
                }
                else
                {
                    arrowImage.enabled = false;
                }
            }
            else
            {
                FindNearestHero();
                if (nearestHero != null)
                {
                    Vector3 direction = nearestHero.position - player.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    arrowImage.rectTransform.localRotation = Quaternion.Euler(0, 0, angle - 90); // Adjust for arrow orientation
                    arrowImage.enabled = true;
                }
                else
                {
                    arrowImage.enabled = false;
                }
            }
        }
        else
        {
            arrowImage.enabled = false;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == troySceneName)
        {
            FindGreekHeroes();
        }
    }

    private bool IsInTroyScene()
    {
        return SceneManager.GetActiveScene().name == troySceneName;
    }

    private void FindGreekHeroes()
    {
        GameObject[] heroes = GameObject.FindGameObjectsWithTag("Boss");
        greekHeroes = new Transform[heroes.Length];
        for (int i = 0; i < heroes.Length; i++)
        {
            greekHeroes[i] = heroes[i].transform;
        }
    }

    private void FindNearestHero()
    {
        float nearestDistance = float.MaxValue;
        nearestHero = null;

        foreach (Transform hero in greekHeroes)
        {
            if (hero != null) // Check if the hero's Transform is not null
            {
                float distance = Vector3.Distance(player.position, hero.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestHero = hero;
                }
            }
        }
    }

    public void CompleteMission()
    {
        missionComplete = true;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        CompleteMission(); // Ensure the compass uses the new target
    }
}
