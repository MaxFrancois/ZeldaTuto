using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaManager : MonoBehaviour
{
    //public Slider ManaSlider;
    public Image ManaBarFiller;
    public Image ManaBarDelayFiller;
    public float DelayFillerSpeed;
    public Inventory PlayerInventory;

    void Start()
    {
        //ManaSlider.maxValue = PlayerInventory.MaxMana;
        //ManaSlider.value = PlayerInventory.MaxMana;
        PlayerInventory.CurrentMana = PlayerInventory.MaxMana;
        ManaBarFiller.fillAmount = PlayerInventory.CurrentMana / PlayerInventory.MaxMana;
    }

    public void AddMana(float amount)
    {
        ManaBarFiller.fillAmount += amount / PlayerInventory.MaxMana;
        PlayerInventory.CurrentMana += amount;
        if (PlayerInventory.CurrentMana > PlayerInventory.MaxMana)
        {
            ManaBarFiller.fillAmount = 1;
            PlayerInventory.CurrentMana = PlayerInventory.MaxMana;
        }
    }

    public void DecreaseMana(float amount)
    {
        ManaBarFiller.fillAmount -= amount / PlayerInventory.MaxMana;
        PlayerInventory.CurrentMana -= amount;
        if (PlayerInventory.CurrentMana <= 0)
        {
            ManaBarFiller.fillAmount = 0;
            PlayerInventory.CurrentMana = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerInventory.CurrentMana < PlayerInventory.MaxMana)
        {
            PlayerInventory.CurrentMana += Time.deltaTime * PlayerInventory.PassiveManaRegenSpeed;
            ManaBarFiller.fillAmount += Time.deltaTime * PlayerInventory.PassiveManaRegenSpeed / PlayerInventory.MaxMana;
            if (PlayerInventory.CurrentMana > PlayerInventory.MaxMana)
            {
                PlayerInventory.CurrentMana = PlayerInventory.MaxMana;
                ManaBarFiller.fillAmount = 1;
            }
        }
        if (ManaBarDelayFiller.fillAmount <= ManaBarFiller.fillAmount)
            ManaBarDelayFiller.fillAmount += Time.deltaTime * DelayFillerSpeed;
        if (ManaBarDelayFiller.fillAmount >= ManaBarFiller.fillAmount)
            ManaBarDelayFiller.fillAmount -= Time.deltaTime * DelayFillerSpeed;
    }
}
