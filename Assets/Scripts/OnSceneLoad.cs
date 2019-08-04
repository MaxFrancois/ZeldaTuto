using System.Collections;
using UnityEngine;

public class OnSceneLoad : MonoBehaviour
{
    [SerializeField] FadePanelSignal fadeFromSignal;
    [SerializeField] VoidSignal updateUISignal;

    void Start()
    {
        updateUISignal.Raise();
        PermanentObjects.Instance.Player.Freeze();
        StartCoroutine(ScreenFade());
    }

    IEnumerator ScreenFade()
    {
        fadeFromSignal.Raise();
        yield return new WaitForSeconds(fadeFromSignal.Duration);
        PermanentObjects.Instance.Player.Unfreeze();
    }
}
