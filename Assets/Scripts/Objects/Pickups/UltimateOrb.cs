using UnityEngine;

public class UltimateOrb : Powerup
{
    public float Quantity;

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
        {
            collidedObject.GetComponent<CharacterUltimate>().GainUltimate(Quantity);
            Destroy(gameObject);
        }
    }
}
