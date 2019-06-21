using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image HealthBarFiller;
    public Image HealthBarDelayFiller;
    public float DelayFillerSpeed;
    //public Inventory PlayerInventory;
    public CharacterHealth PlayerHealth;

    void Start()
    {
        //PlayerHealth.CurrentHealth = PlayerHealth.MaxHealth;
        HealthBarFiller.fillAmount = PlayerHealth.Health.CurrentHealth / PlayerHealth.Health.MaxHealth;
    }

    public void AddHealth(float amount)
    {
        HealthBarFiller.fillAmount += amount / PlayerHealth.Health.MaxHealth;
        //PlayerHealth.CurrentHealth += amount;
        if (PlayerHealth.Health.CurrentHealth > PlayerHealth.Health.MaxHealth)
        {
            HealthBarFiller.fillAmount = 1;
            //PlayerHealth.CurrentHealth = PlayerHealth.MaxHealth;
        }
    }

    public void DecreaseHealth(float amount)
    {
        HealthBarFiller.fillAmount -= amount / PlayerHealth.Health.MaxHealth;
        //PlayerHealth.CurrentHealth -= amount;
        if (PlayerHealth.Health.CurrentHealth <= 0)
        {
            HealthBarFiller.fillAmount = 0;
            //PlayerHealth.CurrentHealth = 0;
        }
    }

    void Update()
    {
        //if (PlayerHealth.CurrentHealth < PlayerHealth.MaxHealth)
        //{
        //    PlayerHealth.CurrentHealth += Time.deltaTime * PlayerHealth.PassiveHealthRegenSpeed;
        //    HealthBarFiller.fillAmount += Time.deltaTime * PlayerHealth.PassiveHealthRegenSpeed / PlayerHealth.MaxHealth;
        //    if (PlayerHealth.CurrentHealth > PlayerHealth.MaxHealth)
        //    {
        //        PlayerHealth.CurrentHealth = PlayerHealth.MaxHealth;
        //        HealthBarFiller.fillAmount = 1;
        //    }
        //}
        if (HealthBarDelayFiller.fillAmount <= HealthBarFiller.fillAmount)
            HealthBarDelayFiller.fillAmount += Time.deltaTime * DelayFillerSpeed;
        if (HealthBarDelayFiller.fillAmount >= HealthBarFiller.fillAmount)
            HealthBarDelayFiller.fillAmount -= Time.deltaTime * DelayFillerSpeed;
    }
}
