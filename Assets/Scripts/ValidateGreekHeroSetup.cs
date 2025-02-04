// Testing script used to ensure that the Greek heroes are set up correctly.

using UnityEngine;

public class ValidateGreekHeroSetup : MonoBehaviour
{
    void Start()
    {
        GameObject[] greekHeroes = GameObject.FindGameObjectsWithTag("Boss");
        foreach (GameObject hero in greekHeroes)
        {
            Debug.Log($"Found Greek Hero: {hero.name}, Tag: {hero.tag}");
        }
    }
}