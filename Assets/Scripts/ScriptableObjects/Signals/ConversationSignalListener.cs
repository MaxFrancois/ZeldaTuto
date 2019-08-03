using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ConversationEvent : UnityEvent<Conversation> { }

public class ConversationSignalListener : SingleSignalListenerBase
{
    public ConversationEvent Event;

    public void OnSignalRaised(Conversation parameter)
    {
        Event.Invoke(parameter);
    }
}
