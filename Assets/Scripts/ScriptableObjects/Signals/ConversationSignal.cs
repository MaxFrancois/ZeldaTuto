using UnityEngine;

[CreateAssetMenu(fileName = "ConversationSignal", menuName = "Signals/ConversationSignal")]
public class ConversationSignal : SignalBase
{
    public void Raise(Conversation parameter)
    {
        for (int i = Listeners.Count - 1; i >= 0; i--)
        {
            (Listeners[i] as ConversationSignalListener).OnSignalRaised(parameter);
        }
    }
}