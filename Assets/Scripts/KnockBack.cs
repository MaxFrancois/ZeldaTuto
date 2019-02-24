using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public float Thrust;
    public float PushTime;
    public float Damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Breakable") && gameObject.CompareTag("Player"))
        {
            collidedObject.GetComponent<Pot>().Destroy();
        }
        if (collidedObject.gameObject.CompareTag("Enemy") || collidedObject.gameObject.CompareTag("Player"))
        {
            var collidedBody = collidedObject.GetComponent<Rigidbody2D>();
            if (collidedBody != null)
            {
                Vector2 difference = collidedBody.transform.position - transform.position;
                difference = difference.normalized * Thrust;
                collidedBody.AddForce(difference, ForceMode2D.Impulse);
                if (collidedObject.gameObject.CompareTag("Enemy") && collidedObject.isTrigger)
                {
                    collidedBody.GetComponent<Enemy>().CurrentState = EnemyState.Staggered;
                    collidedObject.GetComponent<Enemy>().Knock(collidedBody, PushTime, Damage);
                }
                if (collidedObject.gameObject.CompareTag("Player") 
                    && collidedObject.GetComponent<PlayerMovement>().State != PlayerState.Staggered)
                {
                    collidedBody.GetComponent<PlayerMovement>().State = PlayerState.Staggered;
                    collidedObject.GetComponent<PlayerMovement>().Knock(PushTime, Damage);
                }
            }
        }
    }
}
