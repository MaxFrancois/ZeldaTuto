using System.Collections;
using UnityEngine;

public class RestZone : Interactable
{
    public VoidSignal RestZoneSignal;

    protected override void StartInteraction()
    {
        if (!MenuManager.IsPaused)
            StartCoroutine(OpenBook());
    }

    IEnumerator OpenBook()
    {
        animator.SetBool("IsOpen", true);
        yield return new WaitForSeconds(0.5f);
        RestZoneSignal.Raise();
    }
}
