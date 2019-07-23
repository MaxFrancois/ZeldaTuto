using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TimelineEvent : UnityEvent<List<SpellConfig>, Sprite> { }

public class TimelineSignalListener : SingleSignalListenerBase
{
    public TimelineEvent Event;

    public void OnSignalRaised(List<SpellConfig> spells, Sprite sprite)
    {
        Event.Invoke(spells, sprite);
    }
}
