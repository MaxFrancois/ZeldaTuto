using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public FloatValue CurrentHealth;
    public VoidSignal HealthSignal;
    [Header("Movement")]
    public VectorValue StartingPosition;
    public float DashDistance;
    public GameObject DashAnimation;
    [Header("Inventories")]
    public Inventory PlayerInventory;
    private SpellBar Spells;
    [Header("Other")]
    public SpriteRenderer ReceivedItemSprite;
    public VoidSignal PlayerHit;
    public GameObject DamageTakenCanvas;

    private Rigidbody2D rigidBody;
    private Vector3 change;
    private Animator animator;
    private Vector3 currentDirection;

    void Start()
    {
        State = PlayerState.Idle;
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        Spells = GetComponent<SpellBar>();
        animator.SetFloat("MoveX", 0);
        animator.SetFloat("MoveY", -1);
        transform.position = StartingPosition.InitialValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (State == PlayerState.Interacting) return;
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
        else if (Input.GetButtonDown("Spell 0") && State != PlayerState.Attacking && State != PlayerState.Staggered)
        {
            CastSpell(0, currentDirection);
        }
        else if (Input.GetButtonDown("Spell 1") && State != PlayerState.Attacking && State != PlayerState.Staggered)
        {
            CastSpell(1, currentDirection);
        }
        else if (Input.GetButtonDown("Dash") && State != PlayerState.Attacking && State != PlayerState.Staggered)
        {
            Dash();
        }
        else if (State == PlayerState.Walking || State == PlayerState.Idle)
        {
            UpdateMovement();
        }
    }

    private void CastSpell(int spellIndex, Vector3 direction)
    {
        if (PlayerInventory.CurrentMana >= Spells.Spells[spellIndex].ManaCost)
            Spells.CastSpell(spellIndex, transform, direction);
    }

    private bool CanMove(Vector3 direction, float distance)
    {
        //can't find the right collider for map ?
        return Physics2D.Raycast(transform.position, direction, distance).collider == null;
    }

    private void Dash()
    {
        var direction = new Vector3(animator.GetFloat("MoveX"), animator.GetFloat("MoveY"), 0);
        if (CanMove(direction, DashDistance))
        {
            StartCoroutine(DashCo(direction, DashDistance));
        }
    }

    private IEnumerator DashCo(Vector3 direction, float distance)
    {
        var currentPosition = transform.position;
        var dashEffect = Instantiate(DashAnimation, currentPosition, Quaternion.identity);
        dashEffect.transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(direction));
        float dashEffectWidth = 3f;
        dashEffect.transform.localScale = new Vector3(DashDistance / dashEffectWidth, 1f, 1f);
        transform.position += direction * DashDistance;
        yield return new WaitForSeconds(0.1f);
        Destroy(dashEffect);
        yield return null;
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

    public void Knock(float pushTime, float damage)
    {
        CurrentHealth.RuntimeValue -= damage;
        HealthSignal.Raise();
        if (CurrentHealth.RuntimeValue > 0)
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
}
