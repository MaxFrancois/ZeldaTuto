using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthManager : MonoBehaviour
{
    public Slider HealthSlider;
    public TextMeshProUGUI BossName;

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Initialize(float bossMaxHp, string bossName)
    {
        this.gameObject.SetActive(true);
        HealthSlider.maxValue = bossMaxHp;
        HealthSlider.value = bossMaxHp;
        HealthSlider.minValue = 0;
        BossName.text = bossName;
    }

    public void AddHealth(float amount)
    {
        HealthSlider.value += amount;
        if (HealthSlider.value > HealthSlider.maxValue)
            HealthSlider.value = HealthSlider.maxValue;
    }

    public void DecreaseHealth(float amount)
    {
        HealthSlider.value -= amount;
        if (HealthSlider.value < HealthSlider.minValue)
            HealthSlider.value = HealthSlider.minValue;
    }
}
