using System.Collections;
using UnityEngine;

public class RestZone : Interactable
{
    [SerializeField] VoidSignal restZoneSignal;
    bool isOpen = false;

    protected override void StartInteraction()
    {
        if (!MenuManager.IsPaused && !MenuManager.RecentlyUnpaused)
            StartCoroutine(OpenBook());
    }

    IEnumerator OpenBook()
    {
        isOpen = true;
        animator.SetBool("IsOpen", true);
        yield return new WaitForSeconds(0.5f);
        restZoneSignal.Raise();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.isTrigger && isOpen)
            animator.SetBool("IsOpen", false);
    }
}
