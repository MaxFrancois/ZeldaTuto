using UnityEngine;

[RequireComponent(typeof(CharacterHealth))]
[RequireComponent(typeof(CharacterState))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public abstract class EnemyBase : IHasHealth
{
    [Header("Basic")]
    protected Rigidbody2D body;
    protected Transform target;
    protected Animator animator;
    protected CharacterHealth EnemyHealth;
    protected CharacterState EnemyState;
    protected BlinkOnHit BlinkOnHit;
    protected SpriteRenderer spriteRenderer;
    public string Name;
    public float ChaseRadius;
    public float AttackRadius;
    public float MoveSpeed;
    public Collider2D BlockCollider;
    public Collider2D TriggerCollider;
    public Vector2 HomePosition;
    public LootTable LootTable;
    public GameObject EnemyStateUI;
    //public FloatSignal HitSignal;
    public VoidSignal DeadSignal;
    public GameObject DeathAnimation;
    public GameObject DirectionalArrow;
    protected Vector3 CurrentDirection;
    //public FloatValue MaxHealth;
    //protected float currentHealth;
    protected bool IsDead { get { return EnemyHealth.Health.CurrentHealth <= 0; } }
    protected float CurrentHealth { get { return EnemyHealth.Health.CurrentHealth; } }
    protected float MaxHealth { get { return EnemyHealth.Health.MaxHealth; } }

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

    protected void EnableEnemyStateUI()
    {
        if (!EnemyStateUI.activeInHierarchy)
            EnemyStateUI.SetActive(true);
    }

    protected void HideEnemyStateUI()
    {
        if (EnemyStateUI.activeInHierarchy)
            EnemyStateUI.SetActive(false);
    }

    protected virtual void Update()
    {
        if (DirectionalArrow && CurrentDirection != Vector3.zero)
            DirectionalArrow.transform.up = CurrentDirection;
    }

    protected virtual void OnEnable()
    {
        EnemyHealth.Health.CurrentHealth = MaxHealth;
        EnemyState.MovementState = CharacterMovementState.Idle;
    }

    protected virtual void ChangeMovementDirection(Vector2 direction)
    {
        CurrentDirection = direction;
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

    public Health GetEnemyHealth()
    {
        return EnemyHealth.Health;
    }

    protected bool CanAct()
    {
        if ((EnemyState.MovementState != CharacterMovementState.Idle &&
            EnemyState.MovementState != CharacterMovementState.Walking) ||
            MenuManager.IsPaused || MenuManager.RecentlyUnpaused)
            return false;
        return true;
    }
}
