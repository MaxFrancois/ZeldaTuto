using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class FloatEvent : UnityEvent<float> { }

public class FloatSignalListener : SingleSignalListenerBase
{
    public FloatEvent Event;

    public void OnSignalRaised(float parameter)
    {
        Event.Invoke(parameter);
    }
}
