using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectConfig : ScriptableObject
{
    [Header("Global")]
    public string Name;
    public Sprite Icon;
    public float Duration;
}
