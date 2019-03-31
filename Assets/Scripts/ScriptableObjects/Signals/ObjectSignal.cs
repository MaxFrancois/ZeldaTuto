using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectSignal", menuName = "Signals/ObjectSignal")]
public class ObjectSignal : SignalBase
{
    public void Raise(Object parameter)
    {
        for (int i = Listeners.Count - 1; i >= 0; i--)
        {
            (Listeners[i] as ObjectSignalListener).OnSignalRaised(parameter);
        }
    }
}