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

public enum MinotaurMeleeAttacks
{
    None = 0,
    Swing = 1,
    Cleave = 2,
    Shove = 3
}

public enum MinotaurRangeAttacks
{
    None = 0,
    Stomp = 1,
    Charge = 2
}

[System.Serializable]
public class AOEZone
{
    public string ZoneName;
    public Vector3 BottomLeft;
    public Vector3 TopRight;
}

public class Minotaur : MiniBoss
{
    public MinotaurState CurrentState;
    public BoxCollider2D triggerCollider;
    [Header("Attack")]
    public int ChargeAttackDamage;
    public int StompAttackDamage;
    public float ChaseRadius;
    public float AttackRadius;
    public Room AggroRoom;
    public float MaxChasingTime;
    public float MaxMeleeTime;
    [Header("FallingRocks")]
    public GameObject Avalanche;
    public AvalancheConfig AvalancheConfig;
    public float SpawnFrequency;
    private float spawnFrequencyTracker;
    public int SpawnQuantity;
    public float SpawnDuration;
    private float spawnDurationTracker;
    private bool isSpawningRocks;
    private bool hasSpawnedFirstRock;
    private List<Vector3> recentFalls = new List<Vector3>();
    public AOEZone RoomZone;
    public List<AOEZone> AreasToIgnore = new List<AOEZone>();
    public float MinDistanceBetweenRocks;
    public float StunDuration;
    [Header("Movement")]
    public float MoveSpeed;
    public float ChargeSpeed;
    private float lastX;
    private float timeChasing;
    private float timeInMelee;
    private bool inCombat;
    private Vector3 chargingDirection;
    [Header("Camera")]
    public CinemachineVirtualCamera RoomCamera;
    private CinemachineBasicMultiChannelPerlin roomCameraNoise;
    public float cameraShakeDuration;
    public float cameraShakeAmplitude;
    public float cameraShakeFrequency;
    private float cameraShakeCurrentDuration;
    [Header("Enraged")]
    public float EnragePercentage;
    public float EnragedMoveSpeed;
    public int EnragedSpawnQuantity;
    public float EnragedSpawnFrequency;
    private bool isEnraged;
    private bool chargeInProgress = false;

    private MinotaurMeleeAttacks lastMeleeAttackUsed = MinotaurMeleeAttacks.None;
    private MinotaurRangeAttacks lastRangeAttackused = MinotaurRangeAttacks.None;

    //imeplement pathfinding to fix
    private float currentChargeDuration;

    private void Enrage()
    {
        Debug.Log("ENRAGING!");
        //trigger enrage animation
        MoveSpeed = EnragedMoveSpeed;
        SpawnQuantity = EnragedSpawnQuantity;
        SpawnFrequency = EnragedSpawnFrequency;
        isEnraged = true;
    }

    private void Awake()
    {
        chargeInProgress = false;
        currentChargeDuration = 0;
        inCombat = false;
        isSpawningRocks = false;
        isDead = false;
        hasSpawnedFirstRock = false;
        isEnraged = false;
        body = GetComponent<Rigidbody2D>();
        transform.position = HomePosition;
        currentHealth = MaxHealth.InitialValue;
        animator = GetComponent<Animator>();
        Target = GameObject.FindWithTag("Player").transform;
        if (RoomCamera != null)
            roomCameraNoise = RoomCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void StartCombat()
    {
        inCombat = true;
    }

    public void PlayIntroAnimation()
    {
        StartCoroutine(PlayIntroCo());
    }

    IEnumerator PlayIntroCo()
    {
        animator.SetTrigger("Intro");
        yield return new WaitForSeconds(6f);
        animator.ResetTrigger("Intro");
    }

    private void FailedCharge()
    {
        ResetCombatTimers();
        currentChargeDuration = 0;
        animator.SetBool("IsCharging", false);
        ChangeState(MinotaurState.Idle);
    }

    void FixedUpdate()
    {
        if (AggroRoom.ContainsPlayer && inCombat)
        {
            UpdateCameraShake();

            if (currentHealth > 0 && !isDead)
            {
                if (currentChargeDuration > 0)
                    currentChargeDuration -= Time.deltaTime;
                if (currentChargeDuration <= 0 && chargeInProgress && CurrentState == MinotaurState.Charging)
                    FailedCharge();
                if (!isEnraged && currentHealth <= MaxHealth.InitialValue * EnragePercentage / 100)
                    Enrage();
                UpdateFallingRocks();

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
                    body.MovePosition(temp);
                    ChangeState(MinotaurState.Walking);
                    animator.SetBool("IsWalking", true);
                    timeChasing += Time.deltaTime;
                    if (timeChasing > MaxChasingTime)
                    {
                        animator.SetBool("IsWalking", false);

                        if (lastRangeAttackused == MinotaurRangeAttacks.None)
                        {
                            var rangedAttack = Random.Range(1, 2);
                            if (rangedAttack == (int)MinotaurRangeAttacks.Stomp)
                                StartCoroutine(Stomp());
                            else if (rangedAttack == (int)MinotaurRangeAttacks.Charge)
                                StartCoroutine(Scrape());
                        }
                        else
                        {
                            if (lastRangeAttackused == MinotaurRangeAttacks.Charge)
                                StartCoroutine(Stomp());
                            else
                                StartCoroutine(Scrape());
                        }
                        ResetCombatTimers();
                    }
                }
                else if (Vector3.Distance(Target.position, transform.position) < AttackRadius
                    && CurrentState != MinotaurState.Attacking && CurrentState != MinotaurState.Staggered)
                {
                    timeInMelee += Time.deltaTime;
                    if (timeInMelee > MaxMeleeTime)
                    {
                        animator.SetBool("IsWalking", false);
                        var meleeAttack = Random.Range(1, 3);
                        while (meleeAttack == (int)lastMeleeAttackUsed)
                            meleeAttack = Random.Range(1, 3);
                        if (meleeAttack == (int)MinotaurMeleeAttacks.Swing)
                                StartCoroutine(Swing());
                            else if (meleeAttack == (int)MinotaurMeleeAttacks.Cleave)
                                StartCoroutine(Cleave());
                            else if (meleeAttack == (int)MinotaurMeleeAttacks.Shove)
                            StartCoroutine(Shove());
                        ResetCombatTimers();
                    }
                }
                else if (Vector3.Distance(Target.position, transform.position) > ChaseRadius)
                {
                    var temp = Vector3.MoveTowards(transform.position, Target.position, MoveSpeed * Time.deltaTime);
                    ChangeAnim(temp - transform.position);
                    body.MovePosition(temp);
                    ChangeState(MinotaurState.Walking);
                    animator.SetBool("IsWalking", true);
                    //ChangeState(MinotaurState.Idle);
                    //animator.SetBool("IsWalking", false);
                    //ResetCombatTimers();
                }
            }
        } else
        {
            animator.SetBool("IsWalking", false);
            ChangeState(MinotaurState.Idle);
            ResetCombatTimers();
        }
    }

    private void UpdateCameraShake()
    {
        if (cameraShakeCurrentDuration >= 0)
            cameraShakeCurrentDuration -= Time.deltaTime;
        else
        {
            roomCameraNoise.m_FrequencyGain = 0f;
            roomCameraNoise.m_AmplitudeGain = 0f;
            Target.GetComponent<PlayerMovement>().SetFrozenForCutscene(false);
        }
    }

    private void UpdateFallingRocks()
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
            Target.GetComponent<PlayerMovement>().SetFrozenForCutscene(true);
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
        triggerCollider.enabled = false;
        DeadSignal.Raise();
        body.isKinematic = true;
        GetComponent<KnockBack>().IsActive = false;
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
        lastRangeAttackused = MinotaurRangeAttacks.Stomp;
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
                if (count > 10)
                    break;
                var randomx = Random.Range(RoomZone.BottomLeft.x, RoomZone.TopRight.x);
                var randomy = Random.Range(RoomZone.BottomLeft.y, RoomZone.TopRight.y);
                rockSpawnPosition = new Vector3(randomx, randomy, 0);
                var tooCloseToOtherRock = recentFalls.Any(c => Vector3.Distance(c, rockSpawnPosition) < MinDistanceBetweenRocks);
                var tooCloseToForbiddenZones = AreasToIgnore.Any(c => IsVectorInArea(rockSpawnPosition, c));
                if ((!tooCloseToOtherRock  && !tooCloseToForbiddenZones))
                    goodDistance = true;
            }
            recentFalls.Add(rockSpawnPosition);
            rockscript.Cast(null, rockSpawnPosition);
        }
        yield return null;
    }

    private bool IsVectorInArea(Vector3 vector, AOEZone zone)
    {
        if (vector.x >= zone.BottomLeft.x && vector.x <= zone.TopRight.x
            && vector.y >= zone.BottomLeft.y && vector.y <= zone.TopRight.y)
            return true;
        return false;
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
        currentChargeDuration = 3;
        chargeInProgress = true;
        lastRangeAttackused = MinotaurRangeAttacks.Charge;
        yield return null;
    }

    private IEnumerator AfterCharge()
    {
        currentChargeDuration = 0;
        //if charge in wall, doesnt get stuck
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("IsCharging", false);
        animator.SetTrigger("Hit");
        chargingDirection = Vector3.zero;
        yield return new WaitForSeconds(1.5f);
        ResetCombatTimers();
        animator.ResetTrigger("Hit");
        ChangeState(MinotaurState.Idle);
        chargeInProgress = false;
    }

    private IEnumerator Swing()
    {
        ChangeState(MinotaurState.Attacking);
        animator.SetTrigger("Swing");
        yield return new WaitForSeconds(1.5f);
        ChangeState(MinotaurState.Idle);
        animator.ResetTrigger("Swing");
        lastMeleeAttackUsed = MinotaurMeleeAttacks.Swing;
        yield return null;
    }

    private IEnumerator Cleave()
    {
        ChangeState(MinotaurState.Attacking);
        animator.SetTrigger("Cleave");
        yield return new WaitForSeconds(1.5f);
        ChangeState(MinotaurState.Idle);
        animator.ResetTrigger("Cleave");
        lastMeleeAttackUsed = MinotaurMeleeAttacks.Cleave;
        yield return null;
    }

    private IEnumerator Shove()
    {
        ChangeState(MinotaurState.Attacking);
        animator.SetTrigger("Shove");
        yield return new WaitForSeconds(1f);
        ChangeState(MinotaurState.Idle);
        animator.ResetTrigger("Shove");
        lastMeleeAttackUsed = MinotaurMeleeAttacks.Shove;
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

    public override void Knock(Transform thingThatHitYou, float pushTime, float pushForce, float damage)
    {
        if (!isDead)
        {
            if (CurrentState != MinotaurState.Attacking)
            {
                CurrentState = MinotaurState.Staggered;
                Vector2 difference = transform.position - thingThatHitYou.position;
                difference = difference.normalized * pushForce / 3;
                body.AddForce(difference, ForceMode2D.Impulse);
            }
            if (transform.gameObject.activeInHierarchy)
                StartCoroutine(Knockback(pushTime));
            UpdateHealth(damage);
        }
    }

    private void UpdateHealth(float damage)
    {
        currentHealth -= damage;
        HitSignal.Raise(damage);
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

    private IEnumerator Knockback(float pushTime)
    {
        if (body != null)
        {
            animator.SetTrigger("Hit");
            yield return new WaitForSeconds(pushTime);
            animator.ResetTrigger("Hit");
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
                    StartScreenShake();
                }
            }
        }
    }
}
