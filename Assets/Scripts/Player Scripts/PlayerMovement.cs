using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect
{
    public string Name;
    public Sprite Icon;
}

public enum PlayerState
{
    Idle,
    Walking,
    Interacting,
    Attacking,
    Staggered,
    Dashing,
    Jumping
}

[RequireComponent(typeof(CharacterUltimate))]
[RequireComponent(typeof(CharacterHealth))]
[RequireComponent(typeof(CharacterMana))]
public class PlayerMovement : ITime
{
    [Header("Basics")]
    public float speed;
    public PlayerState State;
    [Header("Movement")]
    public VectorValue StartingPosition;
    [Header("Inventories")]
    protected CharacterHealth PlayerHealth;
    protected CharacterMana PlayerMana;
    protected CharacterUltimate PlayerUltimate;
    public Inventory PlayerInventory;
    protected SpellBar Spells;
    [Header("Signals")]
    public FloatSignal LoseHealthSignal;
    public VoidSignal PlayerHit;
    public ObjectSignal RoomSignal;
    [Header("Other")]
    public SpriteRenderer ReceivedItemSprite;
    //public GameObject DamageTakenCanvas;
    private Rigidbody2D rigidBody;
    private Vector3 change;
    private Animator animator;
    private Vector3 currentDirection;
    private List<StatusEffect> statusEffects = new List<StatusEffect>();
    private bool canInteract = false;
    private SpriteMask lightMask;
    private SpriteRenderer playerSpriteRenderer;
    private float defaultSpeed;
    public BoxCollider2D BlockCollider;
    public BoxCollider2D TriggerCollider;
    public GameObject DirectionalArrow;
    private RespawnPoint respawnPoint;
    private BlinkOnHit blinkOnHit;

    void Start()
    {
        defaultSpeed = speed;
        State = PlayerState.Idle;
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        Spells = GetComponent<SpellBar>();
        lightMask = GetComponent<SpriteMask>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        PlayerHealth = GetComponent<CharacterHealth>();
        PlayerMana = GetComponent<CharacterMana>();
        PlayerUltimate = GetComponent<CharacterUltimate>();
        animator.SetFloat("MoveX", 0);
        animator.SetFloat("MoveY", -1);
        transform.position = StartingPosition.InitialValue;
        blinkOnHit = GetComponent<BlinkOnHit>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (State == PlayerState.Interacting || State == PlayerState.Staggered || MenuManager.IsPaused || MenuManager.RecentlyUnpaused) return;
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.H)) { Debug.Log("Pressed Jump " + State); }
        if (State == PlayerState.Jumping)
        {
            UpdateJump();
        }
        else if (Input.GetButtonDown("Attack") && State != PlayerState.Attacking && State != PlayerState.Staggered)
        {
            StartCoroutine(AttackCo());
        }
        else if (Input.GetButtonDown("Ultimate") && State != PlayerState.Attacking && State != PlayerState.Staggered)
        {
            if (PlayerUltimate.Ultimate.CurrentUltimate >= Spells.Ultimate.ManaCost)
            {
                PlayerUltimate.LoseUltimate(Spells.Ultimate.ManaCost);
                Spells.CastUltimate(transform, currentDirection);
            }
        }
        else if (Input.GetButtonDown("Spell 0") && State != PlayerState.Attacking && State != PlayerState.Staggered && !canInteract)
        {
            CastSpell(0, currentDirection);
        }
        else if (Input.GetButtonDown("Spell 1") && State != PlayerState.Attacking && State != PlayerState.Staggered)
        {
            CastSpell(1, currentDirection);
        }
        else if (Input.GetButtonDown("Spell 2") && State != PlayerState.Attacking && State != PlayerState.Staggered)
        {
            CastSpell(2, currentDirection);
        }
        else if (Input.GetButtonDown("Spell 3") && State != PlayerState.Attacking && State != PlayerState.Staggered)
        {
            CastSpell(3, currentDirection);
        }
        else if (Input.GetKeyDown(KeyCode.H) && State != PlayerState.Attacking && State != PlayerState.Staggered)
        {
            Debug.Log("StartingJump");
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.C) && State != PlayerState.Attacking && State != PlayerState.Staggered)
        {
            Debug.Log("spawn circle");
            SpawnCircle();
        }
        else if (State == PlayerState.Walking || State == PlayerState.Idle || State == PlayerState.Jumping)
        {
            UpdateMovement();
        }
    }
    public GameObject circle;
    private void SpawnCircle()
    {
        var spawnposition = new Vector3(transform.position.x + 3, transform.position.y, 0);
        var c = Instantiate(circle, spawnposition, Quaternion.identity);
        //Destroy(c, 5f);
    }

    //private bool isJumping = false;
    public float JumpUpSpeed;
    public float JumpDownSpeed;
    public float JumpTime;
    float currentJumpTime = 0;
    bool goingUp = false;
    bool goingDown = false;
    void Jump()
    {
        Debug.Log("jump coord", rigidBody.transform);
        State = PlayerState.Jumping;
        animator.SetBool("IsJumping", true);
        goingUp = true;
        //isJumping = true;
        currentJumpTime = JumpTime;
    }

    void UpdateJump()
    {
        if (change != Vector3.zero)
        {
            animator.SetFloat("MoveX", change.x);
            animator.SetFloat("MoveY", change.y);
        }
        if (goingUp)
        {
            if (currentJumpTime > 0)
            {
                currentJumpTime -= Time.deltaTime;
                rigidBody.velocity = new Vector2(change.x, change.y + 1) * JumpUpSpeed * Time.deltaTime;
            }
            else
            {
                goingDown = true;
                goingUp = false;
                currentJumpTime = JumpTime;
            }
        }
        else if (goingDown)
        {
            if (currentJumpTime > 0)
            {
                currentJumpTime -= Time.deltaTime;
                rigidBody.velocity = new Vector2(change.x, change.y - 1) * JumpDownSpeed * Time.deltaTime;
            }
            else
            {
                Debug.Log("Landed coord", rigidBody.transform);
                goingDown = false;
                State = PlayerState.Idle;
                rigidBody.velocity = Vector2.zero;
                animator.SetBool("IsJumping", false);
                //isJumping = false;
            }
        }

    }

    //use timer instead
    public void SetFrozenForCutscene(bool isFrozen)
    {
        if (isFrozen)
            State = PlayerState.Staggered;
        else
            State = PlayerState.Idle;
    }

    public void SetCanInteract()
    {
        canInteract = !canInteract;
    }

    public void SetMoveSpeed(float moveSpeedMultiplier)
    {
        if (moveSpeedMultiplier != -1)
        {
            speed = defaultSpeed * moveSpeedMultiplier;
        }
        else
        {
            speed = defaultSpeed;
        }
    }

    private void CastSpell(int spellIndex, Vector3 direction)
    {
        if (Spells.Spells[spellIndex] != null && PlayerMana.Mana.CurrentMana >= Spells.Spells[spellIndex].ManaCost)
        {
            PlayerMana.LoseMana(Spells.Spells[spellIndex].ManaCost);
            Spells.CastSpell(spellIndex, transform, direction);
        }
    }

    private void UpdateMovement()
    {
        if (change != Vector3.zero)
        {
            animator.SetBool("IsMoving", true);
            animator.SetFloat("MoveX", change.x);
            animator.SetFloat("MoveY", change.y);
            currentDirection = change;
            DirectionalArrow.transform.up = change;
            rigidBody.MovePosition(transform.position + change.normalized * speed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("IsAttacking", true);
        State = PlayerState.Attacking;
        yield return null;
        animator.SetBool("IsAttacking", false);
        yield return new WaitForSeconds(.33f);
        if (State != PlayerState.Interacting)
            State = PlayerState.Walking;
    }

    public void ReceiveItem()
    {
        if (PlayerInventory.currentItem != null)
        {
            if (State != PlayerState.Interacting)
            {
                animator.SetBool("IsPickingUp", true);
                State = PlayerState.Interacting;
                ReceivedItemSprite.sprite = PlayerInventory.currentItem.ItemSprite;
            }
            else
            {
                animator.SetBool("IsPickingUp", false);
                State = PlayerState.Idle;
                ReceivedItemSprite.sprite = null;
                PlayerInventory.currentItem = null;
            }
        }
    }

    public void TakeDamage(Transform thingThatHitYou, float pushTime, float pushForce, float damage)
    {
        State = PlayerState.Staggered;
        if (thingThatHitYou)
        {
            Vector2 difference = transform.position - thingThatHitYou.position;
            difference = difference.normalized * pushForce;
            rigidBody.AddForce(difference, ForceMode2D.Impulse);
        }
        //LoseHealthSignal.Raise(damage);
        PlayerHealth.LoseHealth(damage);
        blinkOnHit.Blink(playerSpriteRenderer);
        if (PlayerHealth.Health.CurrentHealth > 0)
        {
            StartCoroutine(KnockbackCo(rigidBody, pushTime, damage));
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator KnockbackCo(Rigidbody2D body, float pushTime, float damage)
    {
        if (body != null)
        {
            TriggerCollider.enabled = false;
            var damageTakenPosition = new Vector3(transform.position.x, transform.position.y + 1, 0);
            //var damageTaken = Instantiate(DamageTakenCanvas, damageTakenPosition, Quaternion.identity);
            //damageTaken.GetComponent<DamageDisplay>().Initialize(damage, false);
            PlayerHit.Raise();
            yield return new WaitForSeconds(pushTime);
            State = PlayerState.Idle;
            body.velocity = Vector2.zero;
            TriggerCollider.enabled = true;
        }
    }

    public void SetSpriteMask(bool isActive)
    {
        if (isActive)
        {
            playerSpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            lightMask.enabled = true;
        }
        else
        {
            playerSpriteRenderer.maskInteraction = SpriteMaskInteraction.None;
            lightMask.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Room"))
        {
            RoomSignal.Raise(collision);
            //WeatherManager.SetRoom(collision);
        }
    }

    public void SetRespawnPoint(RespawnPoint rp)
    {
        respawnPoint = rp;
    }

    public void TriggerFall(float damage, Vector3 fallTowards)
    {
        fallingTowards = fallTowards;
        StartCoroutine(FallCo(damage));
    }

    private void Update()
    {
        if (isShrinking && transform.localScale.x > 0 && fallingTowards != Vector3.zero)
        {
            transform.localScale -= Vector3.one * Time.deltaTime * 0.4f;
            transform.position = Vector3.MoveTowards(transform.position, fallingTowards, 1   * Time.deltaTime);
        }
    }

    Vector3 fallingTowards;
    bool isShrinking = false;
    IEnumerator FallCo(float damage)
    {
        State = PlayerState.Staggered;
        animator.SetBool("IsMoving", false);
        animator.SetBool("IsPickingUp", true);
        isShrinking = true;
        yield return new WaitForSeconds(2);
        fallingTowards = Vector3.zero;
        isShrinking = false;
        transform.localScale = new Vector3(1, 1, 1);
        animator.SetBool("IsPickingUp", false);
        transform.position = respawnPoint.RespawnPosition;
        TakeDamage(null, 0, 0, damage);
        State = PlayerState.Idle;
    }
}
