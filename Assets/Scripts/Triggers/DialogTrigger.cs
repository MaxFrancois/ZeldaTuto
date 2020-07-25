using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : Trigger
{
    [SerializeField] ConversationSignal signal = default;
    [SerializeField] Conversation conversation = default;

    protected override void OnPlayerEnter()
    {
        signal.Raise(conversation);
        //TODO: disable data
        //gameObject.SetActive(false);
    }
}
