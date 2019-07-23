using UnityEngine;
using UnityEngine.UI;

public class Chest : Interactable
{
    [SerializeField] Item Contents;
    [SerializeField] Inventory PlayerInventory;
    [SerializeField] ObjectSignal OnPickupObjectSignal;
    [SerializeField] GameObject DialogWindow;
    [SerializeField] Text DialogText;

    protected override void StartInteraction()
    {
        OpenChest();
    }

    void OpenChest()
    {
        DialogWindow.SetActive(true);
        DialogText.text = Contents.ItemDescription;
        PlayerInventory.AddItem(Contents);
        OnPickupObjectSignal.Raise(Contents.ItemSprite);
        Context.Raise(false);
        animator.SetBool("IsOpened", true);
        Data.CanBeInteractedWith = false;
        canInteract = false;
    }
}
