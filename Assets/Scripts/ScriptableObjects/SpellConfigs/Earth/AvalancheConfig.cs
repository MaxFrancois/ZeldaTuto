using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvalancheConfig", menuName = "SpellConfigs/Earth/AvalancheConfig")]
public class AvalancheConfig : SpellConfig
{
    [Header("Visuals Objects")]
    public GameObject AOEEffect;
    public GameObject FallingRock;
    [Header("Falling Rock")]
    public float FallDistance;
    public float TimeBeforeFall;
    public float FadeSpeed;
    [Header("AOE Circle")]
    public Vector3 MaxZoneSize;
    public Vector3 CircleSize;
    public float ZoneExpandSpeed;

    public override void Cast(Transform source, Vector3 direction)
    {
        var aoe = Instantiate(AOEEffect, direction, Quaternion.identity);
        var aoeScript = aoe.GetComponent<AOEEffect>();
        aoeScript.Initialize(ZoneExpandSpeed, CircleSize, MaxZoneSize, FadeSpeed);

        var rockPosition = new Vector3(direction.x, direction.y + FallDistance);
        var rock = Instantiate(FallingRock, rockPosition, Quaternion.identity);
        var rockScript = rock.GetComponent<FallingRock>();
        rockScript.Initialize(rockPosition, PushForce, PushTime, Damage, FallDistance, TimeBeforeFall, FadeSpeed);
    }
}
