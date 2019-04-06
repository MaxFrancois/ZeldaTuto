using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image HealthBarFiller;
    public Image HealthBarDelayFiller;
    public float DelayFillerSpeed;
    public Inventory PlayerInventory;

    void Start()
    {
        PlayerInventory.CurrentHealth = PlayerInventory.MaxHealth;
        HealthBarFiller.fillAmount = PlayerInventory.CurrentHealth / PlayerInventory.MaxHealth;
    }

    public void AddHealth(float amount)
    {
        HealthBarFiller.fillAmount += amount / PlayerInventory.MaxHealth;
        PlayerInventory.CurrentHealth += amount;
        if (PlayerInventory.CurrentHealth > PlayerInventory.MaxHealth)
        {
            HealthBarFiller.fillAmount = 1;
            PlayerInventory.CurrentHealth = PlayerInventory.MaxHealth;
        }
    }

    public void DecreaseHealth(float amount)
    {
        HealthBarFiller.fillAmount -= amount / PlayerInventory.MaxHealth;
        PlayerInventory.CurrentHealth -= amount;
        if (PlayerInventory.CurrentHealth <= 0)
        {
            HealthBarFiller.fillAmount = 0;
            PlayerInventory.CurrentHealth = 0;
        }
    }

    void Update()
    {
        if (PlayerInventory.CurrentHealth < PlayerInventory.MaxHealth)
        {
            PlayerInventory.CurrentHealth += Time.deltaTime * PlayerInventory.PassiveHealthRegenSpeed;
            HealthBarFiller.fillAmount += Time.deltaTime * PlayerInventory.PassiveHealthRegenSpeed / PlayerInventory.MaxHealth;
            if (PlayerInventory.CurrentHealth > PlayerInventory.MaxHealth)
            {
                PlayerInventory.CurrentHealth = PlayerInventory.MaxHealth;
                HealthBarFiller.fillAmount = 1;
            }
        }
        if (HealthBarDelayFiller.fillAmount <= HealthBarFiller.fillAmount)
            HealthBarDelayFiller.fillAmount += Time.deltaTime * DelayFillerSpeed;
        if (HealthBarDelayFiller.fillAmount >= HealthBarFiller.fillAmount)
            HealthBarDelayFiller.fillAmount -= Time.deltaTime * DelayFillerSpeed;
    }
}
