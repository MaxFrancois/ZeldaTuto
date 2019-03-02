using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Enemy[] Enemies;
    public Pot[] Pots;
    public GameObject VirtualCamera;

    public virtual void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
        {
            foreach (var enemy in Enemies)
            {
                ChangeActivation(enemy, true);
            }

            foreach (var pot in Pots)
            {
                ChangeActivation(pot, true);
            }
            VirtualCamera.SetActive(true);
        }
    }

    public virtual void OnTriggerExit2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
        {
            foreach (var enemy in Enemies)
            {
                ChangeActivation(enemy, false);
            }

            foreach (var pot in Pots)
            {
                ChangeActivation(pot, false);
            }
            VirtualCamera.SetActive(false);
        }
    }

    public void ChangeActivation(Component component, bool setActive)
    {
        component.gameObject.SetActive(setActive);
    }
}
