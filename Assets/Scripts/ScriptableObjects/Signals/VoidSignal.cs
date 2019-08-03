using UnityEngine;

[CreateAssetMenu(fileName = "VoidSignal", menuName = "Signals/VoidSignal")]
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
