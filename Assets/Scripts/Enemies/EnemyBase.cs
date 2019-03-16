using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    protected Rigidbody2D body;

    public virtual void Knock(Transform thingThatHitYou, float pushTime, float pushForce, float damage)
    {

    }
}
