using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

public enum MinotaurState
{
    Idle,
    Walking,
    Charging,
    Attacking,
    Staggered,
    Dead
}

public class Minotaur : MonoBehaviour
{
    [Header("Basic")]
    public MinotaurState CurrentState;
    private float currentHealth;
    public FloatValue MaxHealth;
    public string Name;
    public Vector2 HomePosition;
    private Transform Target;
    public LootTable LootTable;
    private Animator animator;
    private Rigidbody2D body;
    private bool isDead;
    [Header("Attack")]
    public int ChargeAttackDamage;
    public int StompAttackDamage;
    public float ChaseRadius;
    public float AttackRadius;
    [Header("FallingRocks")]
    public GameObject Avalanche;
    public AvalancheConfig AvalancheConfig;
    public Vector2 RoomTopLeft;
    public Vector2 RoomBottomRight;
    public float SpawnFrequency;
    private float spawnFrequencyTracker;
    public int SpawnQuantity;
    public float SpawnDuration;
    private float spawnDurationTracker;
    private bool isSpawningRocks;
    private bool hasSpawnedFirstRock;
    private List<Vector3> recentFalls = new List<Vector3>();
    [Header("Movement")]
    public float MoveSpeed;
    public float ChargeSpeed;
    private float lastX;
    private float timeChasing;
    private float timeInMelee;
    private Vector3 chargingDirection;
    [Header("Camera")]
    public CinemachineVirtualCamera RoomCamera;
    private CinemachineBasicMultiChannelPerlin roomCameraNoise;
    public float cameraShakeDuration;
    public float cameraShakeAmplitude;
    public float cameraShakeFrequency;
    private float cameraShakeCurrentDuration;

    private void Awake()
    {
        isSpawningRocks = false;
        isDead = false;
        hasSpawnedFirstRock = false;
        body = GetComponent<Rigidbody2D>();
        transform.position = HomePosition;
        currentHealth = MaxHealth.InitialValue;
        animator = GetComponent<Animator>();
        Target = GameObject.FindWithTag("Player").transform;
        if (RoomCamera != null)
            roomCameraNoise = RoomCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void FixedUpdate()
    {
        if (cameraShakeCurrentDuration >= 0)
            cameraShakeCurrentDuration -= Time.deltaTime;
        else
        {
            roomCameraNoise.m_FrequencyGain = 0f;
            roomCameraNoise.m_AmplitudeGain = 0f;
        }
        
        if (currentHealth > 0 && !isDead)
        {
            HandleFallingRocks();

            if (CurrentState == MinotaurState.Charging)
            {
                body.MovePosition(transform.position + chargingDirection.normalized * ChargeSpeed * Time.deltaTime);
            }
            else if (Vector3.Distance(Target.position, transform.position) <= ChaseRadius
                && Vector3.Distance(Target.position, transform.position) > AttackRadius
                && CurrentState != MinotaurState.Staggered && CurrentState != MinotaurState.Attacking)
            {
                var temp = Vector3.MoveTowards(transform.position, Target.position, MoveSpeed * Time.deltaTime);
                ChangeAnim(temp - transform.position);
                //body.MovePosition(temp);
                ChangeState(MinotaurState.Walking);
                animator.SetBool("IsWalking", true);
                timeChasing += Time.deltaTime;
                if (timeChasing > 3)
                {
                    animator.SetBool("IsWalking", false);
                    //var rangedAttack = Random.Range(0, 2);
                    //if (rangedAttack == 0)
                        StartCoroutine(Stomp());
                    //else
                    //    StartCoroutine(Scrape());
                    ResetCombatTimers();
                }
            }
            else if (Vector3.Distance(Target.position, transform.position) < AttackRadius
                && CurrentState != MinotaurState.Attacking && CurrentState != MinotaurState.Staggered)
            {
                timeInMelee += Time.deltaTime;
                if (timeInMelee > 1f)
                {
                    animator.SetBool("IsWalking", false);
                    ResetCombatTimers();
                    var meleeAttack = Random.Range(0, 3);
                    if (meleeAttack == 0)
                        StartCoroutine(Swing());
                    else if (meleeAttack == 1)
                        StartCoroutine(Cleave());
                    else
                        StartCoroutine(Shove());
                }
            }
            else if (Vector3.Distance(Target.position, transform.position) > ChaseRadius)
            {
                ChangeState(MinotaurState.Idle);
                animator.SetBool("IsWalking", false);
                ResetCombatTimers();
            }
        }
    }

    private void HandleFallingRocks()
    {
        if (isSpawningRocks)
        {
            var deltaTime = Time.deltaTime;
            spawnDurationTracker -= deltaTime;
            spawnFrequencyTracker -= deltaTime;
            if (spawnFrequencyTracker <= 0 || !hasSpawnedFirstRock)
            {
                hasSpawnedFirstRock = true;
                StartCoroutine(SpawnFallingRocks());
                spawnFrequencyTracker = SpawnFrequency;
            }
            if (spawnDurationTracker <= 0)
            {
                spawnFrequencyTracker = 0;
                isSpawningRocks = false;
                hasSpawnedFirstRock = false;
                recentFalls = new List<Vector3>();
            }
        }
    }

    private void ResetCombatTimers()
    {
        timeInMelee = 0;
        timeChasing = 0;
    }

    private void StartScreenShake()
    {
        if (RoomCamera != null)
        {
            cameraShakeCurrentDuration = cameraShakeDuration;
            roomCameraNoise.m_AmplitudeGain = cameraShakeAmplitude;
            roomCameraNoise.m_FrequencyGain = cameraShakeFrequency;
        }
    }

    private IEnumerator Death()
    {
        ChangeState(MinotaurState.Dead);
        animator.SetTrigger("Die");
        isDead = true;
        yield return null;
    }

    private IEnumerator Stomp()
    {
        ChangeState(MinotaurState.Attacking);
        animator.SetTrigger("Stomp");
        yield return new WaitForSeconds(0.8f);
        StartScreenShake();
        isSpawningRocks = true;
        spawnDurationTracker = SpawnDuration;
        spawnFrequencyTracker = SpawnFrequency;
        yield return new WaitForSeconds(1.5f);
        ChangeState(MinotaurState.Idle);
        animator.ResetTrigger("Stomp");
        //stun player
        yield return null;
    }

    private IEnumerator SpawnFallingRocks()
    {
        for (int i = 0; i < SpawnQuantity; i++)
        {
            var rock = Instantiate(Avalanche, transform.position, Quaternion.identity);
            var rockscript = rock.GetComponent<Avalanche>();
            rockscript.Config = AvalancheConfig;
            //calculate rnadom location
            var goodDistance = false;
            int count = 0;
            Vector3 rockSpawnPosition = Vector3.zero;
            while (!goodDistance)
            {
                count++;
                var randomx = Random.Range(RoomTopLeft.x, RoomBottomRight.x);
                var randomy = Random.Range(RoomBottomRight.y, RoomTopLeft.y);
                rockSpawnPosition = new Vector3(randomx, randomy, 0);
                var tooclose = recentFalls.Any(c => Vector3.Distance(c, rockSpawnPosition) < 1);
                if (!tooclose || count > 10)
                    goodDistance = true;
            }
            recentFalls.Add(rockSpawnPosition);
            rockscript.Cast(null, rockSpawnPosition);
        }
        yield return null;
    }

    private IEnumerator Scrape()
    {
        ChangeState(MinotaurState.Attacking);
        animator.SetTrigger("Scrape");
        chargingDirection = Target.position - transform.position;
        yield return new WaitForSeconds(1.1f);
        ChangeState(MinotaurState.Charging);
        animator.SetBool("IsCharging", true);
        animator.ResetTrigger("Scrape");
        //StartCoroutine(Charge());
        yield return null;
    }

    private IEnumerator AfterCharge()
    {
        animator.SetBool("IsCharging", false);
        animator.SetTrigger("Hit");
        chargingDirection = Vector3.zero;
        yield return new WaitForSeconds(1.5f);
        ResetCombatTimers();
        animator.ResetTrigger("Hit");
        ChangeState(MinotaurState.Idle);
    }

    private IEnumerator Swing()
    {
        ChangeState(MinotaurState.Attacking);
        animator.SetTrigger("Swing");
        yield return new WaitForSeconds(1.5f);
        ChangeState(MinotaurState.Idle);
        animator.ResetTrigger("Swing");
        yield return null;
    }

    private IEnumerator Cleave()
    {
        ChangeState(MinotaurState.Attacking);
        animator.SetTrigger("Cleave");
        yield return new WaitForSeconds(1.5f);
        ChangeState(MinotaurState.Idle);
        animator.ResetTrigger("Cleave");
        yield return null;
    }

    private IEnumerator Shove()
    {
        ChangeState(MinotaurState.Attacking);
        animator.SetTrigger("Shove");
        yield return new WaitForSeconds(1f);
        ChangeState(MinotaurState.Idle);
        animator.ResetTrigger("Shove");
        yield return null;
    }

    public void ChangeAnim(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0) { SetAnimFloat(Vector2.right); }
            else if (direction.x < 0) { SetAnimFloat(Vector2.left); }
        }
        else if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if (direction.y > 0) { SetAnimFloat(Vector2.up); }
            else if (direction.y < 0) { SetAnimFloat(Vector2.down); }
        }
    }

    public void SetAnimFloat(Vector2 setVector)
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

    protected void ChangeState(MinotaurState state)
    {
        if (state != CurrentState)
            CurrentState = state;
    }

    private void OnEnable()
    {
        transform.position = HomePosition;
        currentHealth = MaxHealth.InitialValue;
    }

    public virtual void Knock(Rigidbody2D body, float pushTime, float damage)
    {
        if (!isDead)
        {
            if (transform.gameObject.activeInHierarchy)
                StartCoroutine(Knockback(body, pushTime));
            UpdateHealth(damage);
        }
    }

    private void UpdateHealth(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            GetLoot();
            StartCoroutine(Death());
            //gameObject.SetActive(false);
        }
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
            yield return new WaitForSeconds(pushTime);
            body.velocity = Vector2.zero;
            ChangeState(MinotaurState.Idle);
        }
    }

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (!collidedObject.CompareTag("Room"))
        {
            if (CurrentState == MinotaurState.Charging)
            {
                StartCoroutine(AfterCharge());
                if (collidedObject.CompareTag("Player") && collidedObject.isTrigger)
                {
                    Debug.Log("Charged into player");
                    //knockback player
                }
                else if (collidedObject.CompareTag("WorldCollision"))
                {
                    Debug.Log("Charged into wall");
                    StartScreenShake();
                }
            }
        }
    }
}
