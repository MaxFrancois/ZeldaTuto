using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoolSignal", menuName = "Signals/BoolSignal")]
public class BoolSignal : SignalBase
{
    public void Raise(bool parameter)
    {
        for (int i = Listeners.Count - 1; i >= 0; i--)
        {
            (Listeners[i] as BoolSignalListener).OnSignalRaised(parameter);
        }
    }
}
