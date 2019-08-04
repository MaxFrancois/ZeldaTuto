using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class StringEvent : UnityEvent<string> { }

public class StringSignalListener : SingleSignalListenerBase
{
    public StringEvent Event;

    public void OnSignalRaised(string parameter)
    {
        Event.Invoke(parameter);
    }
}
