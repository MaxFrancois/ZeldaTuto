﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : Interactable
{
    public Item Contents;
    public Inventory PlayerInventory;
    public bool IsOpened;
    public CustomSignal ReceivedItemSignal;
    public GameObject DialogWindow;
    public Text DialogText;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && IsActive)
        {
            if (!IsOpened)
            {
                OpenChest();
            }
            else
            {
                ChestAlreadyOpened();
            }
        }
    }

    public void OpenChest()
    {
        DialogWindow.SetActive(true);
        DialogText.text = Contents.ItemDescription;
        PlayerInventory.currentItem = Contents;
        PlayerInventory.AddItem(Contents);
        IsOpened = true;
        ReceivedItemSignal.Raise();
        Context.Raise();
        anim.SetBool("IsOpened", true);
    }

    public void ChestAlreadyOpened()
    {
        DialogWindow.SetActive(false);
        ReceivedItemSignal.Raise();
    }

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger && !IsOpened)
        {
            Context.Raise();
            IsActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger && !IsOpened)
        {
            IsActive = false;
            Context.Raise();
        }
    }
}
