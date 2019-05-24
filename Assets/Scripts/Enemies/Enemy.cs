using System.Collections;
using System.Linq;
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
    public VoidSignal RoomSignal;
    protected BlinkOnHit onHit;

    protected virtual void Awake()
    {
        transform.position = HomePosition;
        EnemyHealth = GetComponent<CharacterHealth>();
        EnemyHealth.Health.CurrentHealth = EnemyHealth.Health.MaxHealth;
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        onHit = GetComponent<BlinkOnHit>();
        target = GameObject.FindWithTag("Player").transform;
        CurrentState = EnemyState.Idle;
    }

    public override void TakeDamage(Transform thingThatHitYou, float pushTime, float pushForce, float damage, bool display = true)
    {
        CurrentState = EnemyState.Staggered;
        LoseHealth(damage, display);
        Vector2 difference = transform.position - thingThatHitYou.position;
        difference = difference.normalized * pushForce * (1 - SlowTimeCoefficient);
        body.AddForce(difference, ForceMode2D.Impulse);
        if (transform.gameObject.activeInHierarchy)
            StartCoroutine(Knockback(body, pushTime));
    }

    protected void LoseHealth(float damage, bool display)
    {
        EnemyHealth.LoseHealth(damage, display);
        if (IsDead && CurrentState != EnemyState.Dead)
        {
            if (DeadSignal) DeadSignal.Raise();
            if (RoomSignal != null) RoomSignal.Raise();
            GetLoot();
            if (DeathAnimation != null)
                StartCoroutine(DeathAnimationAndDestroy());
            else
                StartCoroutine(DieAndLeaveBody());
        }
    }

    protected void GainHealth(float healing, bool display)
    {
        EnemyHealth.GainHealth(healing, display);
    }

    private IEnumerator DieAndLeaveBody()
    {
        ChangeState(EnemyState.Dead);
        if (animator && animator.parameters.Any(c => c.name == "IsDead") && animator.parameters.Any(c => c.name == "Die"))
        {
            animator.SetTrigger("Die");
            animator.SetBool("IsDead", true);
        }
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

    protected IEnumerator Knockback(Rigidbody2D body, float pushTime)
    {
        if (body != null)
        {
            if (onHit) onHit.Blink(spriteRenderer);
            if (animator && animator.parameters.Any(c => c.name == "Hit"))
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
