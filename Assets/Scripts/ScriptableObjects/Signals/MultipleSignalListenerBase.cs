using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleSignalListenerBase : SignalListenerBase
{
    public SignalBase[] Signals;

    private void OnEnable()
    {
        foreach (var signal in Signals)
            signal.RegisterListener(this);
    }

    private void OnDisable()
    {
        foreach (var signal in Signals)
            signal.DeregisterListener(this);
    }
}
