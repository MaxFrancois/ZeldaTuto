using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StringSignal", menuName = "Signals/StringSignal")]
public class StringSignal : SignalBase
{
    public void Raise(string parameter)
    {
        for (int i = Listeners.Count - 1; i >= 0; i--)
        {
            (Listeners[i] as StringSignalListener).OnSignalRaised(parameter);
        }
    }
}
