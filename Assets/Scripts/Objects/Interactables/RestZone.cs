using System.Collections;
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
}
