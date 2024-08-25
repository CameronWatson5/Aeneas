// This script is used to show the player's stats at the top of the screen.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class AeneasStatsUI : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI speedText;

    private AeneasAttributes playerAttributes;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayerAttributes();
    }

    void FindPlayerAttributes()
    {
        playerAttributes = FindObjectOfType<AeneasAttributes>();
        if (playerAttributes == null)
        {
            Debug.LogWarning("AeneasAttributes not found in the scene!");
        }
    }

    void Update()
    {
        if (playerAttributes != null)
        {
            UpdateUI();
        }
        else
        {
            FindPlayerAttributes();
        }
    }

    void UpdateUI()
    {
        healthText.text = $"HP: {playerAttributes.currentHealth}/{playerAttributes.maxHealth}";
        goldText.text = $"Gold: {playerAttributes.gold}";
        damageText.text = $"Attack: {playerAttributes.damage}";
        armorText.text = $"Armor: {playerAttributes.armor}";
        speedText.text = $"Speed: {playerAttributes.speed}";
    }
}
