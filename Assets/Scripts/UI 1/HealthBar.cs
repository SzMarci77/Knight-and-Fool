using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthBarText;

    Damageable playerDamagable;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player GameObject not found. Make sure the player has the 'Player' tag assigned.");
            return;
        }
        playerDamagable = player.GetComponent<Damageable>();
    }

    private void Start()
    {
        if (playerDamagable == null || healthSlider == null || healthBarText == null)
        {
            Debug.LogWarning("HealthBar: Hiányzó komponens!");
            return;
        }

        // Health bar-t létrehozzuk
        healthSlider.value = CalculateSliderPercentage(playerDamagable.Health, playerDamagable.MaxHealth);
        healthBarText.text = $"HP {playerDamagable.Health} / {playerDamagable.MaxHealth}";
    }

    private void OnEnable()
    {
        if (playerDamagable != null)
            playerDamagable.healthChanged.AddListener(OnPlayerHealthChanged);
    }

    private void OnDisable()
    {
        if (playerDamagable != null)
            playerDamagable.healthChanged.RemoveListener(OnPlayerHealthChanged);
    }

    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }

    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        if (healthSlider != null) healthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        if (healthBarText != null) healthBarText.text = $"HP {newHealth} / {maxHealth}";
    }
}
