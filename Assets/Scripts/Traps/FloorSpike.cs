using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpike : MonoBehaviour
{
    public float Damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.isTrigger)
        {
            collision.GetComponent<PlayerMovement>().TriggerFall(Damage, GetComponent<BoxCollider2D>().bounds.center);
        }
    }
}
