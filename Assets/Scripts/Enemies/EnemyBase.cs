using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : ITime
{
    [Header("Basic")]
    protected Rigidbody2D body;
    protected Transform target;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    public string Name;
    public float ChaseRadius;
    public float AttackRadius;
    public float MoveSpeed;
    public Collider2D BlockCollider;
    public Collider2D TriggerCollider;
    public Vector2 HomePosition;
    public LootTable LootTable;
    public FloatSignal HitSignal;
    public VoidSignal DeadSignal;
    public FloatValue MaxHealth;
    public GameObject DeathAnimation;
    protected float currentHealth;
    protected bool isDead;

    protected float lastX;
    protected bool TargetInChasingRange
    {
        get
        {
            return Vector3.Distance(target.position, transform.position) <= ChaseRadius
                 && Vector3.Distance(target.position, transform.position) > AttackRadius;
        }
    }

    protected bool TargetInAttackRange
    {
        get
        {
            return Vector3.Distance(target.position, transform.position) < AttackRadius;
        }
    }

    protected bool TargetOutOfRange
    {
        get
        {
            return Vector3.Distance(target.position, transform.position) > ChaseRadius;
        }
    }

    public virtual void Knock(Transform thingThatHitYou, float pushTime, float pushForce, float damage)
    {

    }

    protected virtual void AfterAwake()
    {

    }

    protected virtual void ChangeMovementDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0) { SetMovementDirectionFloat(Vector2.right); }
            else if (direction.x < 0) { SetMovementDirectionFloat(Vector2.left); }
        }
        else if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if (direction.y > 0) { SetMovementDirectionFloat(Vector2.up); }
            else if (direction.y < 0) { SetMovementDirectionFloat(Vector2.down); }
        }
    }

    //public virtual void SetMovementDirectionFloat(Vector2 setVector)
    //{
    //    animator.SetFloat("MoveX", setVector.x);
    //    animator.SetFloat("MoveY", setVector.y);
    //}

    protected virtual void SetMovementDirectionFloat(Vector2 setVector)
    {
        if (setVector.x == 0)
            animator.SetFloat("MoveX", lastX);
        else
        {
            animator.SetFloat("MoveX", setVector.x);
            lastX = setVector.x;
        }
        animator.SetFloat("MoveY", setVector.y);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
