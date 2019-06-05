using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    public StatusEffectConfig Config;
    protected float duration;
    public virtual void OnStart(GameObject target) { }
    public virtual void Update() { }
    public virtual void OnFinish() { }
}
