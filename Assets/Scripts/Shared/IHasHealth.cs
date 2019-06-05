using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IHasHealth : ITime
{
    public virtual void TakeDamage(Transform thingThatHitYou, float pushTime, float pushForce, float damage, bool display = true)
    {

    }
}
