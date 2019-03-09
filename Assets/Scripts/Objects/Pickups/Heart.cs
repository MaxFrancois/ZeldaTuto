using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Powerup
{
    public FloatValue PlayerHealth;
    public FloatValue HeartContainers;
    public float AmountToHeal;

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
        {
            PlayerHealth.RuntimeValue += AmountToHeal;
            if (PlayerHealth.RuntimeValue > HeartContainers.RuntimeValue * 2)
            {
                PlayerHealth.RuntimeValue = HeartContainers.RuntimeValue * 2;
            }
            VoidPowerupSignal.Raise();
            Destroy(this.gameObject);
        }
    }
}
