using UnityEngine;

public class CharacterMana : ITime
{
    public FloatSignal OnManaLost;
    public FloatSignal OnManaGained;
    public Mana Mana;

    public void GainMana(float amount)
    {
        Mana.CurrentMana += amount;
        if (Mana.CurrentMana > Mana.MaxMana)
            Mana.CurrentMana = Mana.MaxMana;
        if (OnManaGained) OnManaGained.Raise(amount);
    }

    public void LoseMana(float amount)
    {
        Mana.CurrentMana -= amount;
        if (Mana.CurrentMana < 0)
            Mana.CurrentMana = 0;
        if (OnManaLost) OnManaLost.Raise(amount);
    }

    void Update()
    {
        if (Mana.PassiveManaRegenSpeed != 0)
        {
            var ManaDifference = Mana.PassiveManaRegenSpeed * Time.deltaTime * (1 - SlowTimeCoefficient);
            //Mana.CurrentMana += ManaDifference;
            if (ManaDifference > 0)
            {
                GainMana(ManaDifference);
                //if (Mana.CurrentMana > Mana.MaxMana)
                //    Mana.CurrentMana = Mana.MaxMana;
                //else if (OnManaGained) OnManaGained.Raise(ManaDifference);
            }
            if (ManaDifference < 0)
            {
                LoseMana(-ManaDifference);
                //if (Mana.CurrentMana < 0)
                //    Mana.CurrentMana = 0;
                //else if (OnManaLost) OnManaLost.Raise(ManaDifference);
            }
        }
    }
}
