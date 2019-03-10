using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaManager : MonoBehaviour
{
    public Slider ManaSlider;
    public Inventory PlayerInventory;

    void Start()
    {
        ManaSlider.maxValue = PlayerInventory.MaxMana;
        ManaSlider.value = PlayerInventory.MaxMana;
        PlayerInventory.CurrentMana = PlayerInventory.MaxMana;
    }

    public void AddMana(float amount)
    {
        ManaSlider.value += amount;
        PlayerInventory.CurrentMana += amount;
        if (ManaSlider.value > PlayerInventory.MaxMana)
        {
            ManaSlider.value = PlayerInventory.MaxMana;
            PlayerInventory.CurrentMana = PlayerInventory.MaxMana;
        }
    }

    public void DecreaseMana(float amount)
    {
        ManaSlider.value -= amount;
        PlayerInventory.CurrentMana -= amount;
        if (ManaSlider.value <= 0)
        {
            ManaSlider.value = 0;
            PlayerInventory.CurrentMana = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInventory.CurrentMana += Time.deltaTime * PlayerInventory.PassiveManaRegenSpeed;
        ManaSlider.value += Time.deltaTime * PlayerInventory.PassiveManaRegenSpeed;
        if (PlayerInventory.CurrentMana > PlayerInventory.MaxMana)
            PlayerInventory.CurrentMana = PlayerInventory.MaxMana;
        if (ManaSlider.value > PlayerInventory.MaxMana)
            ManaSlider.value = PlayerInventory.MaxMana;
    }
}
