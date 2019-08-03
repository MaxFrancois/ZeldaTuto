using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterUltimate))]
[RequireComponent(typeof(CharacterHealth))]
[RequireComponent(typeof(CharacterMana))]
public class PlayerMovement : IHasHealth
{
    [SerializeField] PlayerData PlayerData;
    [Header("Basics")]
    public float MoveSpeed;
    private float defaultSpeed;
    public CharacterState PlayerState;
    [Header("Movement")]
    public VectorValue StartingPosition;
    public float InvulnerabilityTime;
    protected PlayerInput PlayerInput;
    [Header("Inventories")]
    protected CharacterHealth PlayerHealth;
    protected CharacterMana PlayerMana;
    protected CharacterUltimate PlayerUltimate;
    public Inventory PlayerInventory;
    protected SpellBar Spells;
    [Header("Signals")]
    public VoidSignal PlayerHit;
    public ObjectSignal RoomSignal;
    [Header("Other")]
    public SpriteRenderer ReceivedItemSprite;
    private Rigidbody2D rigidBody;
    private Animator animator;
    //private Vector3 currentDirection;
    //private bool canInteract = false;
    private SpriteMask lightMask;
    private SpriteRenderer playerSpriteRenderer;
    public BoxCollider2D BlockCollider;
    public BoxCollider2D TriggerCollider;
    public GameObject DirectionalArrow;
    private RespawnPoint respawnPoint;
    private BlinkOnHit blinkOnHit;


    void Awake()
    {
        defaultSpeed = MoveSpeed;
        PlayerState = GetComponent<CharacterState>();
        PlayerInput = GetComponent<PlayerInput>();
        PlayerInput.OnAttack += Attack;
        PlayerInput.OnCastSpell += CastSpell;
        PlayerInput.OnJump += Jump;
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        Spells = GetComponent<SpellBar>();
        lightMask = GetComponent<SpriteMask>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        PlayerHealth = GetComponent<CharacterHealth>();
        PlayerHealth.Initialize();
        PlayerMana = GetComponent<CharacterMana>();
        PlayerUltimate = GetComponent<CharacterUltimate>();
        animator.SetFloat("MoveX", 0);
        animator.SetFloat("MoveY", -1);
        //transform.position = StartingPosition.InitialValue;
        blinkOnHit = GetComponent<BlinkOnHit>();
        LoadData();
    }

    public void Reset()
    {
        LoadData();
    }

    void LoadData()
    {
        transform.position = new Vector3(PlayerData.PlayerPosition.x, PlayerData.PlayerPosition.y, 0);
        Spells.Initialize(PlayerData.Spells);
    }

    private bool CanAct()
    {
        if (PlayerState.MovementState == CharacterMovementState.Stunned ||
            PlayerState.MovementState == CharacterMovementState.Attacking ||
            PlayerState.MovementState == CharacterMovementState.Dead)
            return false;
        return true;
    }

    void FixedUpdate()
    {
        if (!CanAct()) return;
        if (PlayerState.MovementState == CharacterMovementState.Falling && transform.localScale.x > 0 && fallingTowards != Vector3.zero)
        {
            transform.localScale -= Vector3.one * Time.deltaTime * 0.4f;
            transform.position = Vector3.MoveTowards(transform.position, fallingTowards, 1 * Time.deltaTime);
        }
        else if (PlayerState.MovementState == CharacterMovementState.Jumping)
        {
            UpdateJump();
        }
        else if (PlayerState.MovementState == CharacterMovementState.Dashing)
        {
            UpdateDash();
        }
        else
        {
            UpdateMovement();
        }
    }

    #region Falling
    Vector3 fallingTowards;

    public void TriggerFall(float damage, Vector3 fallTowards)
    {
        fallingTowards = fallTowards;
        StartCoroutine(FallCo(damage));
    }

    IEnumerator FallCo(float damage)
    {
        PlayerState.MovementState = CharacterMovementState.Falling;
        animator.SetBool("IsMoving", false);
        animator.SetBool("IsPickingUp", true);
        yield return new WaitForSeconds(2);
        fallingTowards = Vector3.zero;
        transform.localScale = new Vector3(1, 1, 1);
        animator.SetBool("IsPickingUp", false);
        if (respawnPoint)
            transform.position = respawnPoint.RespawnPosition;
        else
            transform.position = StartingPosition.InitialValue;
        TakeDamage(null, 0, 0, damage);
        PlayerState.MovementState = CharacterMovementState.Idle;
    }
    #endregion

    #region Jumping
    public float JumpUpSpeed;
    public float JumpDownSpeed;
    public float JumpTime;
    float currentJumpTime = 0;
    bool goingUp = false;
    bool goingDown = false;
    void Jump()
    {
        Debug.Log("jump coord", rigidBody.transform);
        PlayerState.MovementState = CharacterMovementState.Jumping;
        animator.SetBool("IsJumping", true);
        goingUp = true;
        currentJumpTime = JumpTime;
    }

    void UpdateJump()
    {
        if (PlayerInput.CurrentFacingDirection != Vector3.zero)
        {
            animator.SetFloat("MoveX", PlayerInput.CurrentFacingDirection.x);
            animator.SetFloat("MoveY", PlayerInput.CurrentFacingDirection.y);
        }
        if (goingUp)
        {
            if (currentJumpTime > 0)
            {
                currentJumpTime -= Time.deltaTime;
                rigidBody.velocity = new Vector2(PlayerInput.CurrentFacingDirection.x, PlayerInput.CurrentFacingDirection.y + 1) * JumpUpSpeed * Time.deltaTime;
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
                rigidBody.velocity = new Vector2(PlayerInput.CurrentFacingDirection.x, PlayerInput.CurrentFacingDirection.y - 1) * JumpDownSpeed * Time.deltaTime;
            }
            else
            {
                Debug.Log("Landed coord", rigidBody.transform);
                goingDown = false;
                PlayerState.MovementState = CharacterMovementState.Idle;
                rigidBody.velocity = Vector2.zero;
                animator.SetBool("IsJumping", false);
                //isJumping = false;
            }
        }

    }
    #endregion Jumping

    #region Attacking

    private void Attack()
    {
        StartCoroutine(AttackCo());
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("IsAttacking", true);
        PlayerState.MovementState = CharacterMovementState.Attacking;
        yield return new WaitForSeconds(.33f);
        animator.SetBool("IsAttacking", false);
        PlayerState.MovementState = CharacterMovementState.Idle;
        //if (PlayerState.MovementState != CharacterMovementState.Interacting)
        //    PlayerState.MovementState = CharacterMovementState.Walking;
    }

    #endregion

    #region CastSpells
    private void CastSpell(int spellIndex)
    {
        if (spellIndex == -1)
        {
            if (PlayerUltimate.Ultimate.CurrentUltimate >= Spells.Ultimate.ManaCost)
            {
                PlayerUltimate.LoseUltimate(Spells.Ultimate.ManaCost);
                Spells.CastUltimate(transform, PlayerInput.CurrentFacingDirection);
            }
        }
        else
        {
            if (spellIndex < Spells.Spells.Count && Spells.Spells[spellIndex] != null && PlayerMana.Mana.CurrentMana >= Spells.Spells[spellIndex].ManaCost)
            {
                PlayerMana.LoseMana(Spells.Spells[spellIndex].ManaCost);
                Spells.CastSpell(spellIndex, transform, PlayerInput.CurrentFacingDirection);
            }
        }
    }
    #endregion

    #region Movement

    float dashDuration = 0;
    Vector3 dashDirection;
    Vector2 dashHitPoint;

    private void UpdateDash()
    {
        //if (dashDuration > 0 && dashHitPoint != (Vector2)transform.position)
        //{
        //    dashDuration -= Time.deltaTime * (1 - SlowTimeCoefficient);
        //if (dashHitPoint == Vector2.zero)
        //    rigidBody.MovePosition(transform.position + dashDirection.normalized * MoveSpeed * Time.deltaTime * (1 - SlowTimeCoefficient));
        //else
        //rigidBody.MovePosition(Vector3.Lerp(transform.position, dashHitPoint, MoveSpeed * Time.deltaTime * (1 - SlowTimeCoefficient)));
        //rigidBody.velocity = dashDirection.normalized * MoveSpeed * Time.deltaTime * (1 - SlowTimeCoefficient);
        //}
        if (Vector2.Distance(dashHitPoint, transform.position) > 0.2)
        {
            rigidBody.MovePosition(Vector3.Lerp(transform.position, dashHitPoint, MoveSpeed * Time.deltaTime * (1 - SlowTimeCoefficient)));
        }
        else
        {
            PlayerState.MovementState = CharacterMovementState.Idle;
            MoveSpeed = defaultSpeed;
            rigidBody.velocity = Vector2.zero;
            dashHitPoint = Vector2.zero;
        }
    }

    public void Dash(float duration, float dashSpeed, Vector3 direction)
    {
        DirectionalArrow.transform.up = direction;
        //TODO: better wall detection when dashing, currently gets stuck
        var willHitWall = Physics2D.RaycastAll(transform.position, direction, 5);
        var hit = willHitWall.FirstOrDefault(c => c.collider != null && c.collider.CompareTag("WorldCollision"));
        if (hit && hit.collider)
            dashHitPoint = hit.point;
        else
            dashHitPoint = new Vector2(transform.position.x + (direction.normalized.x * 4), transform.position.y + (direction.normalized.y * 4));
        MoveSpeed = dashSpeed;
        dashDirection = direction;
        dashDuration = duration;
        PlayerState.MovementState = CharacterMovementState.Dashing;
    }

    public void SetMoveSpeed(float moveSpeedMultiplier)
    {
        if (moveSpeedMultiplier != -1)
        {
            MoveSpeed = defaultSpeed * moveSpeedMultiplier;
        }
        else
        {
            MoveSpeed = defaultSpeed;
        }
    }

    private void UpdateMovement()
    {
        if (PlayerInput.CurrentInputDirection != Vector3.zero)
        {
            animator.SetBool("IsMoving", true);
            animator.SetFloat("MoveX", PlayerInput.CurrentInputDirection.x);
            animator.SetFloat("MoveY", PlayerInput.CurrentInputDirection.y);
            DirectionalArrow.transform.up = PlayerInput.CurrentInputDirection;
            rigidBody.MovePosition(transform.position + PlayerInput.CurrentInputDirection.normalized * MoveSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }
    #endregion

    public void Freeze()
    {
        PlayerInput.CurrentInputDirection = Vector3.zero;
        PlayerState.MovementState = CharacterMovementState.Stunned;
        animator.SetBool("IsMoving", false);
    }

    public void Unfreeze()
    {
        PlayerInput.CurrentInputDirection = Vector3.zero;
        PlayerState.MovementState = CharacterMovementState.Idle;
    }

    public void ReceiveItem(MonoBehaviour spriteRenderer)
    {
        //if (PlayerInventory.currentItem != null)
        //{
        if (spriteRenderer)//PlayerState.MovementState != CharacterMovementState.Interacting)
        {
            animator.SetBool("IsPickingUp", true);
            Freeze();
            //ReceivedItemSprite.sprite = (spriteRenderer as SpriteRenderer).sprite;
            
            //PlayerState.MovementState = CharacterMovementState.Interacting;
            //if (sprite)
            //else
            //    ReceivedItemSprite.sprite = PlayerInventory.currentItem.ItemSprite;
        }
        else
        {
            animator.SetBool("IsPickingUp", false);
            //PlayerState.MovementState = CharacterMovementState.Idle;
            ReceivedItemSprite.sprite = null;
            //PlayerInventory.currentItem = null;
            Unfreeze();
        }
        //}
    }

    public void ReceiveItem(object item)
    {
        if (item != null && item is Sprite)
        {
            animator.SetBool("IsPickingUp", true);
            ReceivedItemSprite.sprite = (item as Sprite);
            ReceivedItemSprite.gameObject.SetActive(true);
            Freeze();
        }
        else
        {
            animator.SetBool("IsPickingUp", false);
            ReceivedItemSprite.sprite = null;
            ReceivedItemSprite.gameObject.SetActive(false);
            Unfreeze();
        }
    }


    public override void TakeDamage(Transform thingThatHitYou, float pushTime, float pushForce, float damage, bool display = true)
    {
        if (PlayerState.MovementState == CharacterMovementState.Dashing)
        {
            rigidBody.velocity = Vector2.zero;
            PlayerState.MovementState = CharacterMovementState.Idle;
            MoveSpeed = defaultSpeed;
        }

        PlayerState.MovementState = CharacterMovementState.Stunned;
        if (thingThatHitYou)
        {
            Vector2 difference = transform.position - thingThatHitYou.position;
            difference = difference.normalized * pushForce;
            rigidBody.AddForce(difference, ForceMode2D.Impulse);
        }
        PlayerHealth.LoseHealth(damage, display);
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
            PlayerHit.Raise();
            yield return new WaitForSeconds(pushTime);
            PlayerState.MovementState = CharacterMovementState.Idle;
            body.velocity = Vector2.zero;
            yield return new WaitForSeconds(InvulnerabilityTime - pushTime);
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
        if (collision.CompareTag("WorldCollision"))
        {
            if (PlayerState.MovementState == CharacterMovementState.Dashing)
            {
                dashDuration = 0;
                MoveSpeed = defaultSpeed;
                PlayerState.MovementState = CharacterMovementState.Idle;
                transform.position = dashHitPoint;
                dashHitPoint = Vector2.zero;
                rigidBody.velocity = Vector2.zero;
            }
        }
    }

    public void SetRespawnPoint(RespawnPoint rp)
    {
        respawnPoint = rp;
    }


}
