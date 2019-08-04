using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TimelineConversationEvent : UnityEvent<Conversation, string> { }

public class TimelineConversationSignalListener : SingleSignalListenerBase
{
    public TimelineConversationEvent Event;

    public void OnSignalRaised(Conversation conv, string timelineId)
    {
        Event.Invoke(conv, timelineId);
    }
}
