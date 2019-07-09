using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class BoolEvent : UnityEvent<bool> { }

public class BoolSignalListener : SingleSignalListenerBase
{
    public BoolEvent Event;

    public void OnSignalRaised(bool parameter)
    {
        Event.Invoke(parameter);
    }
}
