using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatSignal", menuName = "Signals/FloatSignal")]
public class FloatSignal : SignalBase
{
    public void Raise(float parameter)
    {
        for (int i = Listeners.Count - 1; i >= 0; i--)
        {
            (Listeners[i] as FloatSignalListener).OnSignalRaised(parameter);
        }
    }
}
