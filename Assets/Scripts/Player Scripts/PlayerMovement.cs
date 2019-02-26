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
    Staggered
}

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rigidBody;
    private Vector3 change;
    private Animator animator;
    public PlayerState State;
    public FloatValue CurrentHealth;
    public CustomSignal HealthSignal;
    public VectorValue StartingPosition;
    public Inventory PlayerInventory;
    public SpriteRenderer ReceivedItemSprite;
    public CustomSignal PlayerHit;

    void Start()
    {
        State = PlayerState.Idle;
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
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
        if (Input.GetButtonDown("Attack") && State != PlayerState.Attacking && State != PlayerState.Staggered)
        {
            StartCoroutine(AttackCo());
        }
        else if (State == PlayerState.Walking || State == PlayerState.Idle)
        {
            UpdateMovement();
        }
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
            PlayerHit.Raise();
            yield return new WaitForSeconds(pushTime);
            body.velocity = Vector2.zero;
            State = PlayerState.Idle;
            body.velocity = Vector2.zero;
        }
    }
}
