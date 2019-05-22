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
    public Mana PlayerMana;

    void Start()
    {
        //ManaSlider.maxValue = PlayerInventory.MaxMana;
        //ManaSlider.value = PlayerInventory.MaxMana;
        //PlayerMana.CurrentMana = PlayerMana.MaxMana;
        ManaBarFiller.fillAmount = PlayerMana.CurrentMana / PlayerMana.MaxMana;
    }

    public void AddMana(float amount)
    {
        ManaBarFiller.fillAmount += amount / PlayerMana.MaxMana;
        //PlayerMana.CurrentMana += amount;
        if (PlayerMana.CurrentMana > PlayerMana.MaxMana)
        {
            ManaBarFiller.fillAmount = 1;
            //PlayerMana.CurrentMana = PlayerMana.MaxMana;
        }
    }

    public void DecreaseMana(float amount)
    {
        ManaBarFiller.fillAmount -= amount / PlayerMana.MaxMana;
        //PlayerMana.CurrentMana -= amount;
        if (PlayerMana.CurrentMana <= 0)
        {
            ManaBarFiller.fillAmount = 0;
            //PlayerMana.CurrentMana = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (PlayerMana.CurrentMana < PlayerMana.MaxMana)
        //{
        //    PlayerMana.CurrentMana += Time.deltaTime * PlayerMana.PassiveManaRegenSpeed;
        //    ManaBarFiller.fillAmount += Time.deltaTime * PlayerMana.PassiveManaRegenSpeed / PlayerMana.MaxMana;
        //    if (PlayerMana.CurrentMana > PlayerMana.MaxMana)
        //    {
        //        PlayerMana.CurrentMana = PlayerMana.MaxMana;
        //        ManaBarFiller.fillAmount = 1;
        //    }
        //}
        if (ManaBarDelayFiller.fillAmount <= ManaBarFiller.fillAmount)
            ManaBarDelayFiller.fillAmount += Time.deltaTime * DelayFillerSpeed;
        if (ManaBarDelayFiller.fillAmount >= ManaBarFiller.fillAmount)
            ManaBarDelayFiller.fillAmount -= Time.deltaTime * DelayFillerSpeed;
    }
}
