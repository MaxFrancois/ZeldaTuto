using UnityEngine;

public class HealthOrb : Powerup
{
    public float Quantity;

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
        {
            collidedObject.GetComponent<CharacterHealth>().GainHealth(Quantity, true);
            Destroy(gameObject);
        }
    }
}
