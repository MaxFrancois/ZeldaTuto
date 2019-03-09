using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBottle : Powerup
{
    public float AmountToRecover;
    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && collidedObject.isTrigger)
        {
            FloatPowerupSignal.Raise(AmountToRecover);
            Destroy(this.gameObject);
        }
    }
}
