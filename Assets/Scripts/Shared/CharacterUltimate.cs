using UnityEngine;

public class CharacterUltimate : ITime
{
    public FloatSignal OnUltimateLost;
    public FloatSignal OnUltimateGained;
    public Ultimate Ultimate;

    public void GainUltimate(float amount)
    {
        Ultimate.CurrentUltimate += amount;
        if (Ultimate.CurrentUltimate > Ultimate.MaxUltimate)
            Ultimate.CurrentUltimate = Ultimate.MaxUltimate;
        if (OnUltimateGained) OnUltimateGained.Raise(amount);
    }

    public void LoseUltimate(float amount)
    {
        Ultimate.CurrentUltimate -= amount;
        if (Ultimate.CurrentUltimate < 0)
            Ultimate.CurrentUltimate = 0;
        if (OnUltimateLost) OnUltimateLost.Raise(amount);
    }

    void Update()
    {
        if (Ultimate.PassiveUltimateRegenSpeed != 0)
        {
            var UltimateDifference = Ultimate.PassiveUltimateRegenSpeed * Time.deltaTime * (1 - SlowTimeCoefficient);
            //Ultimate.CurrentUltimate += UltimateDifference;
            if (UltimateDifference > 0)
            {
                GainUltimate(UltimateDifference);
                //if (Ultimate.CurrentUltimate > Ultimate.MaxUltimate)
                //    Ultimate.CurrentUltimate = Ultimate.MaxUltimate;
                //else if (OnUltimateGained) OnUltimateGained.Raise(UltimateDifference);
            }
            if (UltimateDifference < 0)
            {
                LoseUltimate(-UltimateDifference);
                //if (Ultimate.CurrentUltimate < 0)
                //    Ultimate.CurrentUltimate = 0;
                //else if (OnUltimateLost) OnUltimateLost.Raise(UltimateDifference);
            }
        }
    }
}
