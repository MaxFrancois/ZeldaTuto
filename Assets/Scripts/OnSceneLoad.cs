using System.Collections;
using UnityEngine;

public class OnSceneLoad : MonoBehaviour
{
    [SerializeField] FadePanelSignal fadeFromSignal;

    //void Awake()
    //{
    //    PermanentObjects.Instance.Player.Freeze();
    //    StartCoroutine(ScreenFade());
    //}

    void Start()
    {
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
