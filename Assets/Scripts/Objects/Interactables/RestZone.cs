﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestZone : Interactable
{
    public VoidSignal RestZoneSignal;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact") && IsActive && !MenuManager.IsPaused)
        {
            //RestMenu.SetActive(true);
            //Context.Raise();
            StartCoroutine(OpenBook());
        }
    }

    IEnumerator OpenBook()
    {
        animator.SetBool("IsOpen", true);
        yield return new WaitForSeconds(0.5f);
        RestZoneSignal.Raise();
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
            animator.SetBool("IsOpen", false);
        }
    }
}