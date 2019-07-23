using UnityEngine;

public class HealthInfo
{
    public float MaxHealth;
    public float CurrentHealth;
    public float PassiveHealthRegenSpeed;
}

public class CharacterHealth : ITime
{
    public FloatSignal OnHealthLost;
    public FloatSignal OnHealthGained;
    public DamageDisplay DamageDisplayCanvas;
    public Health HealthSO;
    public HealthInfo Health;

    private void Awake()
    {
        if (Health == null)
            Initialize();
    }

    public void Initialize()
    {
        Health = new HealthInfo();
        Health.MaxHealth = HealthSO.MaxHealth;
        Health.CurrentHealth = HealthSO.MaxHealth;
        Health.PassiveHealthRegenSpeed = HealthSO.PassiveHealthRegenSpeed;
    }

    public void HealToMax()
    {
        Health.CurrentHealth = HealthSO.MaxHealth;
    }

    public void GainHealth(float amount, bool display = true)
    {
        float actualHealing = Health.CurrentHealth + amount > Health.MaxHealth ? Health.MaxHealth - Health.CurrentHealth : amount;
        if (actualHealing != 0)
        {
            Health.CurrentHealth += actualHealing;
            //if (Health.CurrentHealth > Health.MaxHealth)
            //    Health.CurrentHealth = Health.MaxHealth;
            //else
            //{
                if (OnHealthGained) OnHealthGained.Raise(actualHealing);
                if (DamageDisplayCanvas && display)
                {
                    var canvas = Instantiate(DamageDisplayCanvas, transform.position, Quaternion.identity);
                    canvas.Initialize(amount, true);
                }
            //}
        }
    }

    public void LoseHealth(float amount, bool display = true)
    {
        Health.CurrentHealth -= amount;
        if (Health.CurrentHealth < 0)
            Health.CurrentHealth = 0;
        if (OnHealthLost) OnHealthLost.Raise(amount);
        if (DamageDisplayCanvas && display)
        {
            var canvas = Instantiate(DamageDisplayCanvas, transform.position, Quaternion.identity);
            canvas.Initialize(amount, false);
        }
    }

    void Update()
    {
        if (Health.PassiveHealthRegenSpeed != 0)
        {
            var healthDifference = Health.PassiveHealthRegenSpeed * Time.deltaTime * (1 - SlowTimeCoefficient);
            //Health.CurrentHealth += healthDifference;
            if (healthDifference > 0)
            {
                GainHealth(healthDifference, false);
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
