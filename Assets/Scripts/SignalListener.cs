using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalListener : MonoBehaviour
{
    public CustomSignal Signal;
    public UnityEvent Event;

    public void OnSignalRaised()
    {
        Event.Invoke();
    }

    private void OnEnable()
    {
        Signal.RegisterListener(this);
    }

    private void OnDisable()
    {
        Signal.DeregisterListener(this);
    }
}
