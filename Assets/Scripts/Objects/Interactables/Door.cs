using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType
{
    Key,
    Enemy,
    Button
}

public class Door : Interactable
{
    [Header("Door Variables")]
    [Tooltip("Test Tooltip")]
    public DoorType DoorType;
    public bool IsOpen = false;
    public SpriteRenderer SpriteRenderer;
    public Inventory PlayerInventory;
    public BoxCollider2D PhysicsCollider;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact") && IsActive && !IsOpen)
        {
            if (DoorType == DoorType.Key && PlayerInventory.NumberOfKeys > 0)
            {
                Open();
            }
        }
    }

    public virtual void Open()
    {
        // inherit and only use key or other required item on child
        PlayerInventory.NumberOfKeys--;
        SpriteRenderer.enabled = false;
        IsActive = false;
        IsOpen = true;
        PhysicsCollider.enabled = false;
    }

    public void Close()
    {
        SpriteRenderer.enabled = true;
        IsActive = true;
        IsOpen = false;
        PhysicsCollider.enabled = true;
    }

    protected override bool CanInteract()
    {
        return !IsOpen;
    }
}
