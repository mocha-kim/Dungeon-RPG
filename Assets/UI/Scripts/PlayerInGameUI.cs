using System.Collections;
using System.Collections.Generic;
using RPG.StatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInGameUI : MonoBehaviour
{
    public StatsObject playerStats;
    public TextMeshProUGUI levelText;
    public Slider healthSlider;
    public Slider manaSlider;

    // Start is called before the first frame update
    void Start()
    {
        levelText.text = playerStats.level.ToString("n0");

        healthSlider.value = playerStats.HealthPercentage;
        manaSlider.value = playerStats.ManaPercentage;
    }

    private void OnEnable()
    {
        playerStats.OnChangedStats += OnChangedStats;
    }

    private void OnDisable()
    {
        playerStats.OnChangedStats -= OnChangedStats;
    }

    private void OnChangedStats(StatsObject stats)
    {
        levelText.text = playerStats.level.ToString("n0");

        healthSlider.value = playerStats.HealthPercentage;
        manaSlider.value = playerStats.ManaPercentage;
    }
}
