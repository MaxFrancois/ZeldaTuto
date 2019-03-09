using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalListenerBase : MonoBehaviour
{
    public SignalBase Signal;

    private void OnEnable()
    {
        Signal.RegisterListener(this);
    }

    private void OnDisable()
    {
        Signal.DeregisterListener(this);
    }
}
