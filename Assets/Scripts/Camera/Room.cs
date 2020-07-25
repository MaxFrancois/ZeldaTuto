using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] protected Enemy[] Enemies = default;
    [SerializeField] protected Pot[] Pots = default;
    [SerializeField] protected CinemachineVirtualCamera VirtualCamera = default;
    [SerializeField] protected bool ContainsPlayer = default;
    

    public virtual void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
        {
            ContainsPlayer = true;
            foreach (var enemy in Enemies)
            {
                ChangeActivation(enemy, true);
            }

            foreach (var pot in Pots)
            {
                ChangeActivation(pot, true);
            }
            VirtualCamera.gameObject.SetActive(true);
            VirtualCamera.Follow = PermanentObjects.Instance.Player.transform;
        }
    }

    public virtual void OnTriggerExit2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
        {
            ContainsPlayer = false;
            foreach (var enemy in Enemies)
            {
                ChangeActivation(enemy, false);
            }

            foreach (var pot in Pots)
            {
                ChangeActivation(pot, false);
            }
            VirtualCamera.gameObject.SetActive(false);
        }
    }

    public void ChangeActivation(Component component, bool setActive)
    {
        component.gameObject.SetActive(setActive);
    }

    public bool IsPlayerIn()
    {
        return ContainsPlayer;
    }
}
