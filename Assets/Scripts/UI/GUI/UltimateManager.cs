using UnityEngine;
using UnityEngine.UI;

public class UltimateManager : MonoBehaviour
{
    public Image UltimateOrb;
    public Ultimate PlayerUltimate;

    void Start()
    {
        UltimateOrb.fillAmount = PlayerUltimate.CurrentUltimate / PlayerUltimate.MaxUltimate;
    }

    public void IncreaseUltimate(float amount)
    {
        UltimateOrb.fillAmount += amount / PlayerUltimate.MaxUltimate;
        //PlayerUltimate.CurrentUltimate += amount;
        if (PlayerUltimate.CurrentUltimate > PlayerUltimate.MaxUltimate)
        {
            UltimateOrb.fillAmount = 1;
            //PlayerUltimate.CurrentUltimate = PlayerUltimate.MaxUltimate;
        }
    }

    public void SpendUltimate(float amount)
    {
        UltimateOrb.fillAmount -= amount / PlayerUltimate.MaxUltimate;
        //PlayerUltimate.CurrentUltimate  -= amount;
        if (PlayerUltimate.CurrentUltimate <= 0)
        {
            UltimateOrb.fillAmount = 0;
            //PlayerUltimate.CurrentUltimate = 0;
        }
    }

    void Update()
    {
        if (PlayerUltimate.PassiveUltimateRegenSpeed != 0)
            if (PlayerUltimate.PassiveUltimateRegenSpeed > 0)
                IncreaseUltimate(Time.deltaTime * PlayerUltimate.PassiveUltimateRegenSpeed);
            else
                SpendUltimate(Time.deltaTime * PlayerUltimate.PassiveUltimateRegenSpeed);
    }
}
