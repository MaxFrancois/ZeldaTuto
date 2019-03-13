using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvalancheConfig", menuName = "SpellConfigs/AvalancheConfig")]
public class AvalancheConfig : ScriptableObject
{
    public float PushTime;
    public float PushForce;
    public float Damage;
    public float FallDistance;
    public float TimeBeforeFall;
    public float FadeSpeed;
    public Vector3 MaxZoneSize;
    public Vector3 CircleSize;
    public float ZoneExpandSpeed;
}
