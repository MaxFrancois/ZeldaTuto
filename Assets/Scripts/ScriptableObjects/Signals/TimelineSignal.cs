using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimelineSignal", menuName = "Signals/TimelineSignal")]
public class TimelineSignal : SignalBase
{
    public void Raise(List<SpellConfig> parameter, Sprite sprite)
    {
        for (int i = Listeners.Count - 1; i >= 0; i--)
        {
            (Listeners[i] as TimelineSignalListener).OnSignalRaised(parameter, sprite);
        }
    }
}
