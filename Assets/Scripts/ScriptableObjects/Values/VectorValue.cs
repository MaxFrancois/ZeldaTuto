using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VectorValue", menuName = "Values/VectorValue")]
public class VectorValue : ScriptableObject, ISerializationCallbackReceiver
{
    public Vector2 InitialValue;
    public Vector2 DefaultValue;

    public void OnAfterDeserialize()
    {
         InitialValue = DefaultValue;
    }

    public void OnBeforeSerialize()
    {
    }
}
