using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class FloatEvent : UnityEvent<float> { }

public class FloatSignalListener : SignalListenerBase
{
    public FloatEvent Event;

    public void OnSignalRaised(float parameter)
    {
        Event.Invoke(parameter);
    }
}
