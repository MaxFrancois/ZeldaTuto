using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public BoolSignal Context;
    public bool IsActive;

    protected virtual void OnTriggerExit2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
        {
            if (Context != null && gameObject.activeInHierarchy)
            {
                Context.Raise(false);
                IsActive = false;
            }
        }
    }

    protected virtual bool CanInteract()
    {
        return true;
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger && CanInteract())
        {
            if (collision.GetComponent<PlayerInput>().CanInteract() != null)
            {
                IsActive = true;
                Context.Raise(true);
            }
            else
            {
                IsActive = false;
                Context.Raise(false);
            }
        }
    }
}
