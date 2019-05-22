using UnityEngine;

public class Coin : Powerup
{
    public int Quantity;

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
        {
            collidedObject.GetComponent<PlayerMovement>().PlayerInventory.GainCoins(Quantity);
            Destroy(gameObject);
        }
    }
}
