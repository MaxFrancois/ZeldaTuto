using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestZone : Interactable
{
    //public GameObject RestMenu;
    public VoidSignal RestZoneSignal;

    void Update()
    {
        if (Input.GetButtonDown("Interact") && IsActive && !MenuManager.IsPaused)
        {
            //RestMenu.SetActive(true);
            //Context.Raise();
            RestZoneSignal.Raise();
        }
    }

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
        {
            IsActive = true;
            Context.Raise();
        }
    }

    private void OnTriggerExit2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
        {
            Context.Raise();
            IsActive = false;
        }
    }
}
