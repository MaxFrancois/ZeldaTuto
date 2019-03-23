using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoolValue", menuName = "Values/BoolValue")]
public class BoolValue : ScriptableObject, ISerializationCallbackReceiver
{
    public bool InitialValue;
    public bool RuntimeValue;
    public void OnAfterDeserialize()
    {
        RuntimeValue = InitialValue;
    }

    public void OnBeforeSerialize()
    {

    }
}
