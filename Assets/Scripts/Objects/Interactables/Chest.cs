using UnityEngine;
using UnityEngine.UI;

public class Chest : Interactable
{
    [SerializeField] Item Contents = default;
    [SerializeField] Inventory PlayerInventory = default;
    [SerializeField] ObjectSignal OnPickupObjectSignal = default;
    [SerializeField] GameObject DialogWindow = default;
    [SerializeField] Text DialogText = default;

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
