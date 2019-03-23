using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticFire : MonoBehaviour
{
    private float duration;
    private float pushTime;
    private float pushForce;
    private float damage;
    private Animator animator;
    private bool isDestroyed = false;
    private bool ready;
    
    void Awake()
    {
        ready = false;
        animator = GetComponent<Animator>();
    }

    public void Initialize(float dmg, float pusht, float pushf, float dur, float delay)
    {
        duration = dur;
        pushForce = pushf;
        pushTime = pusht;
        damage = dmg;
        StartCoroutine(DelayStart(delay));
    }

    IEnumerator DelayStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        ready = true;
    }

    void Update()
    {
        if (!isDestroyed && ready)
        {
            duration -= Time.deltaTime;
            if (duration <= 0)
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
            if ((collidedObject.gameObject.CompareTag("Enemy") || collidedObject.gameObject.CompareTag("MiniBoss"))
                && collidedObject.isTrigger)
            {
                collidedObject.GetComponent<EnemyBase>().Knock(transform, pushForce, pushTime, damage);
            }
    }
}
