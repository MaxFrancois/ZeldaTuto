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
    public Inventory PlayerInventory;
    public BoxCollider2D PhysicsCollider;

    protected override void StartInteraction()
    {
        if (DoorType == DoorType.Key && PlayerInventory.NumberOfKeys > 0 && !IsOpen)
        {
            Open();
        }
    }

    public virtual void Open()
    {
        // inherit and only use key or other required item on child
        PlayerInventory.NumberOfKeys--;
        spriteRenderer.enabled = false;
        IsPlayerInRange = false;
        IsOpen = true;
        PhysicsCollider.enabled = false;
    }

    public void Close()
    {
        spriteRenderer.enabled = true;
        IsPlayerInRange = true;
        IsOpen = false;
        PhysicsCollider.enabled = true;
    }
}
