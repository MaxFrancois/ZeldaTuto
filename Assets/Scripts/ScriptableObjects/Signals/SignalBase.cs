using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SignalBase : ScriptableObject
{
    public List<SignalListenerBase> Listeners = new List<SignalListenerBase>();

    public void RegisterListener(SignalListenerBase listener)
    {
        if (!Listeners.Contains(listener))
            Listeners.Add(listener);
    }

    public void DeregisterListener(SignalListenerBase listener)
    {
        if (Listeners.Contains(listener))
            Listeners.Remove(listener);
    }
}
