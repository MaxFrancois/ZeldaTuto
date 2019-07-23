using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterHealth))]
[RequireComponent(typeof(CharacterState))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public abstract class EnemyBase : IHasHealth
{
    [Header("Enemy Data")]
    [SerializeField] protected EnemyData Data;
    [Header("Basic")]
    protected Rigidbody2D body;
    //private Transform _target;
    protected Transform target;// { get { if (!_target) _target = PermanentObjects.Instance.Player.transform; return _target; } } 
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

    protected virtual void Awake()
    {
        if (Data == null)
        {
            Debug.LogError("Enemy " + name + " doesn't have its EnemyData scriptableObject");
            UnityEditor.EditorApplication.isPlaying = false;
            return;
        }
        Initialize();
    }
    
    void Initialize()
    {
        if (string.IsNullOrWhiteSpace(Data.EnemyId))
        {
            Data.EnemyId = Guid.NewGuid().ToString();
            gameObject.SetActive(Data.IsAliveDefaultValue);
        }
        target = PermanentObjects.Instance.Player.transform;
        EnemyHealth = GetComponent<CharacterHealth>();
        EnemyHealth.Initialize();
        EnemyState = GetComponent<CharacterState>();
        //EnemyHealth.Health.CurrentHealth = EnemyHealth.Health.MaxHealth;
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        BlinkOnHit = GetComponent<BlinkOnHit>();
        EnemyState.MovementState = CharacterMovementState.Idle;
        if (!Data.IsAlive)
            if (Data.LeaveBody)
            {
                transform.position = Data.BodyPosition;
                EnemyHealth.LoseHealth(EnemyHealth.Health.CurrentHealth, false);
            }
            else
                gameObject.SetActive(Data.IsAlive);
    }

    public bool IsDead
    {
        get
        {
            if (EnemyHealth)
                return EnemyHealth.Health.CurrentHealth <= 0;
            else
                return true;
        }
    }
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
        if (EnemyStateUI && !EnemyStateUI.activeInHierarchy)
            EnemyStateUI.SetActive(true);
    }

    protected void HideEnemyStateUI()
    {
        if (EnemyStateUI && EnemyStateUI.activeInHierarchy)
            EnemyStateUI.SetActive(false);
    }

    protected virtual void Update()
    {
        if (!CanAct()) body.velocity = Vector2.zero;
        if (DirectionalArrow && CurrentDirection != Vector3.zero)
            DirectionalArrow.transform.up = CurrentDirection;
        if (EnemyState.MovementState == CharacterMovementState.Falling && transform.localScale.x > 0 && fallingTowards != Vector3.zero)
        {
            transform.localScale -= Vector3.one * Time.deltaTime * 0.4f;
            transform.position = Vector3.MoveTowards(transform.position, fallingTowards, 1 * Time.deltaTime);
        }
    }

    protected virtual void OnEnable()
    {
        //transform.position = HomePosition;
        Initialize();
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

    public HealthInfo GetEnemyHealth()
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

    Vector3 fallingTowards;

    public void TriggerFall(Vector3 fallTowards)
    {
        fallingTowards = fallTowards;
        StartCoroutine(FallCo());
    }

    IEnumerator FallCo()
    {
        EnemyState.MovementState = CharacterMovementState.Falling;
        animator.SetBool("IsMoving", false);
        yield return new WaitForSeconds(2);
        fallingTowards = Vector3.zero;
        transform.localScale = new Vector3(1, 1, 1);
        TakeDamage(null, 0, 0, MaxHealth);
    }

    public string ID { get { return Data.EnemyId; } }

    public void Reset()
    {
        transform.position = HomePosition;
        EnemyHealth.HealToMax();
        Data.BodyPosition = Vector2.zero;
    }
}
