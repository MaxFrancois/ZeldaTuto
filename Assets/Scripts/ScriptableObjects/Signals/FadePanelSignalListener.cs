using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class FadePanelEvent : UnityEvent<FadeType, Color, int> { }

public class FadePanelSignalListener : MultipleSignalListenerBase
{
    public FadePanelEvent Event;

    public void OnSignalRaised(FadeType type, Color c, int duration)
    {
        Event.Invoke(type, c, duration);
    }
}
