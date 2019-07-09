using UnityEngine;

[CreateAssetMenu(fileName = "ObjectSignal", menuName = "Signals/ObjectSignal")]
public class ObjectSignal : SignalBase
{
    public void Raise(object parameter)
    {
        for (int i = Listeners.Count - 1; i >= 0; i--)
        {
            (Listeners[i] as ObjectSignalListener).OnSignalRaised(parameter);
        }
    }
}