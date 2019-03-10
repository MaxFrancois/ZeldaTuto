using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltimateManager : MonoBehaviour
{
    public Image UltimateOrb;
    public Inventory PlayerInventory;

    void Start()
    {
        UltimateOrb.fillAmount = 0;
        PlayerInventory.CurrentUltimate  = 0;
    }

    public void IncreaseUltimate(float amount)
    {
        UltimateOrb.fillAmount += amount / 100;
        PlayerInventory.CurrentUltimate += amount;
        if (PlayerInventory.CurrentUltimate > PlayerInventory.MaxUltimate)
        {
            UltimateOrb.fillAmount = 1;
            PlayerInventory.CurrentUltimate = PlayerInventory.MaxUltimate;
        }
    }

    public void SpendUltimate(float amount)
    {
        UltimateOrb.fillAmount -= amount / 100;
        PlayerInventory.CurrentUltimate  -= amount;
        if (PlayerInventory.CurrentUltimate <= 0)
        {
            UltimateOrb.fillAmount = 0;
            PlayerInventory.CurrentUltimate = 0;
        }
    }

    void Update()
    {
        if (PlayerInventory.PassiveUltimateRegenSpeed != 0)
            IncreaseUltimate(Time.deltaTime * PlayerInventory.PassiveUltimateRegenSpeed);
    }
}
