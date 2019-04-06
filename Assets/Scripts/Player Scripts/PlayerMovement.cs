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
    Dashing
}

public class PlayerMovement : MonoBehaviour
{
    [Header("Basics")]
    public float speed;
    public PlayerState State;
    [Header("Movement")]
    public VectorValue StartingPosition;
    [Header("Inventories")]
    public Inventory PlayerInventory;
    private SpellBar Spells;
    [Header("Signals")]
    public FloatSignal LoseHealthSignal;
    public VoidSignal PlayerHit;
    public ObjectSignal RoomSignal;
    [Header("Other")]
    public SpriteRenderer ReceivedItemSprite;
    public GameObject DamageTakenCanvas;
    private Rigidbody2D rigidBody;
    private Vector3 change;
    private Animator animator;
    private Vector3 currentDirection;
    private List<StatusEffect> statusEffects = new List<StatusEffect>();
    private bool isFrozenForCutscene;
    private bool canInteract = false;
    private SpriteMask lightMask;
    private SpriteRenderer playerSpriteRenderer;

    void Start()
    {
        isFrozenForCutscene = false;
        State = PlayerState.Idle;
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        Spells = GetComponent<SpellBar>();
        lightMask = GetComponent<SpriteMask>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        animator.SetFloat("MoveX", 0);
        animator.SetFloat("MoveY", -1);
        transform.position = StartingPosition.InitialValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (State == PlayerState.Interacting || isFrozenForCutscene || MenuManager.IsPaused || MenuManager.RecentlyUnpaused) return;
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        var currentDirection = new Vector3(animator.GetFloat("MoveX"), animator.GetFloat("MoveY"), 0);
        if (Input.GetButtonDown("Attack") && State != PlayerState.Attacking && State != PlayerState.Staggered)
        {
            StartCoroutine(AttackCo());
        }
        else if (Input.GetButtonDown("Ultimate") && State != PlayerState.Attacking && State != PlayerState.Staggered)
        {
            if (PlayerInventory.CurrentUltimate >= Spells.Ultimate.ManaCost)
                Spells.CastUltimate(transform, currentDirection);
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
        else if (State == PlayerState.Walking || State == PlayerState.Idle)
        {
            UpdateMovement();
        }
    }

    //use timer instead
    public void SetFrozenForCutscene(bool isFrozen)
    {
        isFrozenForCutscene = isFrozen;
    }

    public void SetCanInteract()
    {
        canInteract = !canInteract;
    }

    private void CastSpell(int spellIndex, Vector3 direction)
    {
        if (Spells.Spells[spellIndex] != null && PlayerInventory.CurrentMana >= Spells.Spells[spellIndex].ManaCost)
            Spells.CastSpell(spellIndex, transform, direction);
    }

    private void UpdateMovement()
    {
        if (change != Vector3.zero)
        {
            animator.SetBool("IsMoving", true);
            animator.SetFloat("MoveX", change.x);
            animator.SetFloat("MoveY", change.y);
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

    public void Knock(Transform thingThatHitYou, float pushTime, float pushForce, float damage)
    {
        State = PlayerState.Staggered;
        Vector2 difference = transform.position - thingThatHitYou.position;
        difference = difference.normalized * pushForce;
        rigidBody.AddForce(difference, ForceMode2D.Impulse);
        LoseHealthSignal.Raise(damage);
        if (PlayerInventory.CurrentHealth > 0)
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
            var damageTakenPosition = new Vector3(transform.position.x, transform.position.y + 1, 0);
            var damageTaken = Instantiate(DamageTakenCanvas, damageTakenPosition, Quaternion.identity);
            damageTaken.GetComponent<DamageDisplay>().DamageNumber = damage;
            PlayerHit.Raise();
            yield return new WaitForSeconds(pushTime);
            State = PlayerState.Idle;
            body.velocity = Vector2.zero;
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
}
