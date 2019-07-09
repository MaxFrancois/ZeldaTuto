using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ObjectEvent : UnityEvent<object> { }

public class ObjectSignalListener : SingleSignalListenerBase
{
    public ObjectEvent Event;

    public void OnSignalRaised(object parameter)
    {
        Event.Invoke(parameter);
    }
}
