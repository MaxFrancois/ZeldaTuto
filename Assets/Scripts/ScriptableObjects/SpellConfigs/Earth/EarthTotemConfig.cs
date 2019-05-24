using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EarthTotemConfig", menuName = "SpellConfigs/Earth/EarthTotemConfig")]

public class EarthTotemConfig : SpellConfig
{
    [Header("Totem")]
    public GameObject Totem;
    public GameObject Circle;
    public float TimeBetweenAttacks;
    public float DeathFadeSpeed;
    public float Healing;

    public override void Cast(Transform source, Vector3 direction)
    {
        // adjust spawn position around player
        var instance = Instantiate(Totem, source.position, Quaternion.identity);
        instance.GetComponent<EarthTotem>().Initialize(this);
    }
}
