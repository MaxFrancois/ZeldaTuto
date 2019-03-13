using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticFire : MonoBehaviour
{
    public float Duration;
    public float PushTime;
    public float PushForce;
    public float Damage;
    private Animator animator;
    private bool isDestroyed = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Initialize(float damage, float pushTime, float pushForce, float duration)
    {
        Duration = duration;
        PushForce = pushForce;
        PushTime = pushTime;
        Damage = damage;
    }

    void Update()
    {
        if (!isDestroyed)
        {
            Duration -= Time.deltaTime;
            if (Duration <= 0)
            {
                isDestroyed = true;
                animator.SetBool("IsBurning", false);
                Destroy(this.gameObject, 0.3f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (!isDestroyed)
            if (collidedObject.gameObject.CompareTag("Enemy") && collidedObject.isTrigger)
            {
                Vector2 difference = collidedObject.transform.position - transform.position;
                difference = difference.normalized * PushForce;
                var collidedBody = collidedObject.GetComponent<Rigidbody2D>();
                collidedBody.AddForce(difference, ForceMode2D.Impulse);
                collidedBody.GetComponent<Enemy>().CurrentState = EnemyState.Staggered;
                collidedObject.GetComponent<Enemy>().Knock(collidedBody, PushTime, Damage);
            }
    }
}
