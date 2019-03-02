using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public CustomSignal Context;
    public bool IsActive;

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
        {
            if (Context != null && gameObject.activeInHierarchy)
            {
                Context.Raise();
            }
            IsActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
        {
            if (Context != null && gameObject.activeInHierarchy)
            {
                Context.Raise();
            }
            IsActive = false;
        }
    }
}
