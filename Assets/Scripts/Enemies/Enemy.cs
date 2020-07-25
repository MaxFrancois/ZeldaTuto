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
    public VoidSignal RoomSignal;

    public override void TakeDamage(Transform thingThatHitYou, float pushTime, float pushForce, float damage, bool display = true)
    {
        EnableEnemyStateUI();
        if (pushTime > 0)
            EnemyState.MovementState = CharacterMovementState.Stunned;
        LoseHealth(damage, display);
        if (transform.gameObject.activeInHierarchy)
        {
            if (thingThatHitYou && pushForce > 0)
            {
                Vector2 difference = transform.position - thingThatHitYou.position;
                difference = difference.normalized * pushForce * (1 - SlowTimeCoefficient);
                body.AddForce(difference, ForceMode2D.Impulse);
            }
            if (BlinkOnHit) BlinkOnHit.Blink(spriteRenderer);
            if (animator && animator.parameters.Any(c => c.name == "Hit"))
                animator.SetTrigger("Hit");
            if (pushTime > 0)
                StartCoroutine(Knockback(body, pushTime));
        }
    }

    protected IEnumerator Knockback(Rigidbody2D body, float pushTime)
    {
        if (body != null)
        {
            yield return new WaitForSeconds(pushTime);
            body.velocity = Vector2.zero;
            EnemyState.MovementState = CharacterMovementState.Idle;
        }
    }

    protected void LoseHealth(float damage, bool display)
    {
        EnableEnemyStateUI();
        EnemyHealth.LoseHealth(damage, display);
        if (IsDead && EnemyState.MovementState != CharacterMovementState.Dead)
        {
            if (!IsRoomEnemy)
            {
                Data.IsAlive = false;
                Data.BodyPosition = transform.position;
            }
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
        EnemyState.MovementState = CharacterMovementState.Dead;
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
        EnemyState.MovementState = CharacterMovementState.Dead;
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
}
