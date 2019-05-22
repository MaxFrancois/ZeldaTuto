using UnityEngine;

public class ManaOrb : Powerup
{
    public float Quantity;

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
        {
            collidedObject.GetComponent<CharacterMana>().GainMana(Quantity);
            Destroy(gameObject);
        }
    }
}
