using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public float Thrust;
    public float PushTime;
    public float Damage;
    public bool IsActive;

    private void Awake()
    {
        IsActive = true;
    }

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (IsActive)
        {
            if (collidedObject.CompareTag("Breakable") && gameObject.CompareTag("Player"))
            {
                collidedObject.GetComponent<Pot>().Destroy();
            }
            if (collidedObject.gameObject.CompareTag("Enemy") || collidedObject.gameObject.CompareTag("Player")
                || collidedObject.gameObject.CompareTag("MiniBoss"))
            {
                var collidedBody = collidedObject.GetComponent<Rigidbody2D>();
                if (collidedBody != null)
                {
                    Vector2 difference = collidedBody.transform.position - transform.position;
                    difference = difference.normalized * Thrust;
                    collidedBody.AddForce(difference, ForceMode2D.Impulse);
                    if ((collidedObject.gameObject.CompareTag("Enemy") || collidedObject.gameObject.CompareTag("MiniBoss"))
                        && collidedObject.isTrigger)
                    {
                        collidedObject.GetComponent<EnemyBase>().Knock(transform, PushTime, Thrust, Damage);
                    }
                    if (collidedObject.gameObject.CompareTag("Player")
                        && collidedObject.GetComponent<PlayerMovement>().State != PlayerState.Staggered)
                    {
                        collidedObject.GetComponent<PlayerMovement>().Knock(transform, PushTime, Thrust, Damage);
                    }
                }
            }
        }
    }
}
