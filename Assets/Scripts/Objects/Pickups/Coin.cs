using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Powerup
{
    public FloatValue PlayerCoins;
    public Inventory PlayerInventory;
    public float AmountToAdd;

    private void Awake()
    {
        VoidPowerupSignal.Raise();
    }

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
        {
            PlayerInventory.Coins++;
            PlayerCoins.RuntimeValue += AmountToAdd;
            VoidPowerupSignal.Raise();
            Destroy(this.gameObject);
        }
    }
}
