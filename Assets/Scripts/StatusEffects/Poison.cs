using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : StatusEffect
{
    IHasHealth characterHealth;
    CharacterState characterState;
    SpriteRenderer spriteRenderer;
    BlinkOnHit blinkOnHit;
    float currentTick;

    public Poison(PoisonConfig cfg)
    {
        Config = cfg;
        duration = cfg.Duration;
    }

    public override void OnStart(GameObject target)
    {
        characterHealth = target.GetComponent<IHasHealth>();
        characterState = target.GetComponent<CharacterState>();
        spriteRenderer = target.GetComponent<SpriteRenderer>();
        blinkOnHit = target.GetComponent<BlinkOnHit>();
        blinkOnHit.SetRegularColor((Config as PoisonConfig).PoisonedEnemyColor);
        spriteRenderer.color = (Config as PoisonConfig).PoisonedEnemyColor;
        characterState.AddStatusEffect(this);
    }

    public override void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
            OnFinish();
        else
        {
            currentTick -= Time.deltaTime;
            if (currentTick <= 0)
            {
                currentTick = (Config as PoisonConfig).DotTickSpeed;
                characterHealth.TakeDamage(null, 0, 0, (Config as PoisonConfig).DotTickDamage);
            }
        }
    }

    public override void OnFinish()
    {
        var baseColor = new Color(1, 1, 1, 1);
        spriteRenderer.color = baseColor;
        blinkOnHit.SetRegularColor(baseColor);
        characterState.RemoveStatusEffect(this);
    }
}
