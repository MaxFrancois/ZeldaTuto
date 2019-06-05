using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : Interactable
{
    public Item Contents;
    public Inventory PlayerInventory;
    public bool IsOpened;
    public VoidSignal ReceivedItemSignal;
    public GameObject DialogWindow;
    public Text DialogText;
    private Animator anim;
    public BoolValue StoredOpen;

    void Start()
    {
        anim = GetComponent<Animator>();
        IsOpened = StoredOpen.RuntimeValue;
        if (IsOpened)
        {
            anim.SetBool("IsOpened", true);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact") && IsActive)
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
        Context.Raise(false);
        anim.SetBool("IsOpened", true);
        StoredOpen.RuntimeValue = IsOpened;
    }

    public void ChestAlreadyOpened()
    {
        DialogWindow.SetActive(false);
        ReceivedItemSignal.Raise();
    }

    protected override bool CanInteract()
    {
        return !IsOpened;
    }
}
