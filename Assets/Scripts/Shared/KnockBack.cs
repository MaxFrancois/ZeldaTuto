using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public float PushForce;
    public float PushTime;
    public float Damage;
    [HideInInspector]
    public bool IsActive;

    private void Awake()
    {
        IsActive = true;
    }

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (IsActive)
        {
            if (collidedObject.CompareTag("Breakable"))// && gameObject.CompareTag("Player"))
            {
                collidedObject.GetComponent<Pot>().Destroy();
            }
            if ((collidedObject.gameObject.CompareTag("Enemy") || collidedObject.gameObject.CompareTag("Player")) && collidedObject.isTrigger)
            {
                var collidedBody = collidedObject.GetComponent<Rigidbody2D>();
                if (collidedBody != null)
                {
                    //Vector2 difference = collidedBody.transform.position - transform.position;
                    //difference = difference.normalized * PushForce;
                    //collidedBody.AddForce(difference, ForceMode2D.Impulse);
                    if (collidedObject.gameObject.CompareTag("Enemy"))
                    {
                        collidedObject.GetComponent<EnemyBase>().TakeDamage(transform, PushTime, PushForce, Damage);
                    }
                    if (collidedObject.gameObject.CompareTag("Player")
                        && collidedObject.GetComponent<PlayerMovement>().State != PlayerState.Staggered)
                    {
                        collidedObject.GetComponent<PlayerMovement>().TakeDamage(transform, PushTime, PushForce, Damage);
                    }
                }
            }
        }
    }
}
