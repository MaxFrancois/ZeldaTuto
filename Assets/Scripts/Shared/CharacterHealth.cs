using UnityEngine;

public class CharacterHealth : ITime
{
    public FloatSignal OnHealthLost;
    public FloatSignal OnHealthGained;
    public Health Health;

    public void GainHealth(float amount)
    {
        Health.CurrentHealth += amount;
        if (Health.CurrentHealth > Health.MaxHealth)
            Health.CurrentHealth = Health.MaxHealth;
        if (OnHealthGained) OnHealthGained.Raise(amount);
    }

    public void LoseHealth(float amount)
    {
        Health.CurrentHealth -= amount;
        if (Health.CurrentHealth < 0)
            Health.CurrentHealth = 0;
        if (OnHealthLost) OnHealthLost.Raise(amount);
    }

    void Update()
    {
        if (Health.PassiveHealthRegenSpeed != 0)
        {
            var healthDifference = Health.PassiveHealthRegenSpeed * Time.deltaTime * (1 - SlowTimeCoefficient);
            //Health.CurrentHealth += healthDifference;
            if (healthDifference > 0)
            {
                GainHealth(healthDifference);
                //if (Health.CurrentHealth > Health.MaxHealth)
                //    Health.CurrentHealth = Health.MaxHealth;
                //else if (OnHealthGained) OnHealthGained.Raise(healthDifference);
            }
            if (healthDifference < 0)
            {
                //if (Health.CurrentHealth < 0)
                //    Health.CurrentHealth = 0;
                //else if (OnHealthLost) OnHealthLost.Raise(healthDifference);
                LoseHealth(-healthDifference);
            }
        }
    }
}
