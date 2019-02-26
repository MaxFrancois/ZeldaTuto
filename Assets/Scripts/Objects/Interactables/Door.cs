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
        if (Input.GetKeyDown(KeyCode.E) && IsActive && !IsOpen)
        {
            if (DoorType == DoorType.Key && PlayerInventory.NumberOfKeys > 0)
            {
                Open();
            }
        }
    }

    public void Open()
    {
        PlayerInventory.NumberOfKeys--;
        SpriteRenderer.enabled = false;
        IsActive = false;
        IsOpen = true;
        PhysicsCollider.enabled = false;
    }

    void Close()
    {

    }
}
