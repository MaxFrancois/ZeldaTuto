using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VoidSignalListener : SignalListenerBase
{
    public UnityEvent Event;

    public void OnSignalRaised()
    {
        Event.Invoke();
    }
}
