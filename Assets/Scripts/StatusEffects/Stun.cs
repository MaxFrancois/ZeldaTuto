using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : StatusEffect
{
    CharacterState characterState;

    public Stun(StunConfig cfg)
    {
        Config = cfg;
        duration = cfg.Duration;
    }

    public override void OnStart(GameObject target)
    {
        characterState = target.GetComponent<CharacterState>();
        characterState.MovementState = CharacterMovementState.Stunned;
        characterState.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        characterState.AddStatusEffect(this);
    }

    public override void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
            OnFinish();
    }

    public override void OnFinish()
    {
        characterState.MovementState = CharacterMovementState.Idle;
        characterState.RemoveStatusEffect(this);
    }
}
