using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Walking,
    Attacking,
    Staggered,
    Dead
}

public class Enemy : EnemyBase
{
    public EnemyState CurrentState;
    public GameObject DeathAnimation;
    public VoidSignal RoomSignal;
    BlinkOnHit onHit;

    private void Awake()
    {
        transform.position = HomePosition;
        currentHealth = MaxHealth.InitialValue;
        isDead = false;
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        onHit = GetComponent<BlinkOnHit>();
        target = GameObject.FindWithTag("Player").transform;
        CurrentState = EnemyState.Idle;
        AfterAwake();
    }

    private void OnEnable()
    {
        transform.position = HomePosition;
        currentHealth = MaxHealth.InitialValue;
    }

    public override void Knock(Transform thingThatHitYou, float pushTime, float pushForce, float damage)
    {
        CurrentState = EnemyState.Staggered;
        UpdateHealth(damage);
        Vector2 difference = transform.position - thingThatHitYou.position;
        difference = difference.normalized * pushForce;
        body.AddForce(difference, ForceMode2D.Impulse);
        if (transform.gameObject.activeInHierarchy)
            StartCoroutine(Knockback(body, pushTime));
    }

    private void UpdateHealth(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            if (DeadSignal) DeadSignal.Raise();
            if (RoomSignal != null) RoomSignal.Raise();
            GetLoot();
            if (DeathAnimation != null)
                StartCoroutine(DeathAnimationAndDestroy());
            else
                StartCoroutine(DieAndLeaveBody());
        }
    }

    private IEnumerator DieAndLeaveBody()
    {
        ChangeState(EnemyState.Dead);
        animator.SetTrigger("Die");
        animator.SetBool("IsDead", true);
        if (TriggerCollider) TriggerCollider.enabled = false;
        if (BlockCollider) BlockCollider.enabled = false;
        body.isKinematic = true;
        yield return null;
    }

    private IEnumerator DeathAnimationAndDestroy()
    {
        var deathAnim = Instantiate(DeathAnimation, transform.position, Quaternion.identity);
        Destroy(deathAnim, 3f);
        gameObject.SetActive(false);
        yield return null;
    }

    private void GetLoot()
    {
        if (LootTable != null)
        {
            var loot = LootTable.GetLoot();
            if (loot != null)
                Instantiate(loot.gameObject, transform.position, Quaternion.identity);
        }
    }

    private IEnumerator Knockback(Rigidbody2D body, float pushTime)
    {
        if (body != null)
        {
            if (onHit) onHit.Blink(spriteRenderer);
            if (animator)
                animator.SetTrigger("Hit");
            yield return new WaitForSeconds(pushTime);
            //animator.ResetTrigger("Hit");
            body.velocity = Vector2.zero;
            CurrentState = EnemyState.Idle;
        }
    }

    protected void ChangeState(EnemyState state)
    {
        if (state != CurrentState)
            CurrentState = state;
    }
}
