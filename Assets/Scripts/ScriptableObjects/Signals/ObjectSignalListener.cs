using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ObjectEvent : UnityEvent<Object> { }

public class ObjectSignalListener : SignalListenerBase
{
    public ObjectEvent Event;

    public void OnSignalRaised(Object parameter)
    {
        Event.Invoke(parameter);
    }
}
