using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : Interactable
{
    public GameObject DialogBox;
    public Text DialogText;
    public string DialogString;

    void Update()
    {
        if (Input.GetButtonDown("Interact") && IsActive)
        {
            if (DialogBox.activeInHierarchy) DialogBox.SetActive(false);
            else
            {
                DialogBox.SetActive(true);
                DialogText.text = DialogString;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
        {
            Context.Raise();
            IsActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
        {
            IsActive = false;
            DialogBox.SetActive(false);
            Context.Raise();
        }
    }
}
