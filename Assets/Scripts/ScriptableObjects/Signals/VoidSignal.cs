using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VoidSignal : SignalBase
{
    public void Raise()
    {
        for (int i = Listeners.Count - 1; i >= 0; i--)
        {
            (Listeners[i] as VoidSignalListener).OnSignalRaised();
        }
    }
}
