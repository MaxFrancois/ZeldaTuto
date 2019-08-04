using UnityEngine;

[CreateAssetMenu(fileName = "TimelineConversationSignal", menuName = "Signals/TimelineConversationSignal")]
public class TimelineConversationSignal : SignalBase
{
    public void Raise(Conversation conv, string timelineId)
    {
        for (int i = Listeners.Count - 1; i >= 0; i--)
        {
            (Listeners[i] as TimelineConversationSignalListener).OnSignalRaised(conv, timelineId);
        }
    }
}