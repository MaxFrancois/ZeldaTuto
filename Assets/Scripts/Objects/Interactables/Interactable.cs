using System;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected InteractableData Data;
    [SerializeField] protected BoolSignal Context;
    [NonSerialized ] protected Animator animator;

    //[SerializeField] protected Sprite SpriteAfterInteraction;

    protected PlayerInput playerInput;
    protected SpriteRenderer spriteRenderer;
    protected bool canInteract;
    protected bool IsPlayerInRange;

    [HideInInspector]
    public string ID { get { return Data.InteractableId; } }

    protected virtual void Awake()
    {
        if (Data == null)
        {
            Debug.LogError("Interactable " + name + " doesn't have its InteractableData scriptableObject");
            UnityEditor.EditorApplication.isPlaying = false;
            return;
        }
    
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        InitializeData();
    }

    void InitializeData()
    {
        canInteract = Data.CanBeInteractedWith;
        if (string.IsNullOrWhiteSpace(Data.InteractableId))
        {
            Data.InteractableId = Guid.NewGuid().ToString();
            gameObject.SetActive(Data.CanBeInteractedWithDefaultValue);
        }
        else if (!Data.CanBeInteractedWith) SetCannotInteract();
        //if (Data.CanBeInteractedWith == Data.CanBeInteractedWithDefaultValue)
        //{
        //    if (Data.CanBeInteractedWithDefaultValue)
        //        return;
        //    else
        //        SetCannotInteract();
        //}
        //else
        //    SetCannotInteract();
    }

    void SetCannotInteract()
    {
        if (Data.HideWhenCannotInteract) gameObject.SetActive(false);
        else
        {
            if (animator) animator.enabled = false;
            if (spriteRenderer) spriteRenderer.sprite = Data.SpriteAfterInteraction;
        }
    }

    //protected virtual void OnTriggerExit2D(Collider2D collidedObject)
    //{
    //    if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
    //    {
    //        if (Context != null && gameObject.activeInHierarchy)
    //        {
    //            Context.Raise(false);
    //            IsActive = false;
    //        }
    //    }
    //}

    protected virtual void Update()
    {
        if (Input.GetButtonDown("Interact") && IsPlayerInRange && canInteract)
        {
            StartInteraction();
        }
    }

    protected virtual void StartInteraction()
    {
        Debug.LogError("StartInteraction not configured for "+ name);
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger && canInteract)
        {
            if (playerInput == null)
                playerInput = collision.GetComponent<PlayerInput>();
            if (playerInput.CanInteract() != null)
            {
                IsPlayerInRange = true;
                Context.Raise(true);
            }
            else
            {
                IsPlayerInRange = false;
                Context.Raise(false);
            }
        }
    }

    public virtual void Reset()
    {
        InitializeData();
    }
}
