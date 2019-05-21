using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireTotemConfig", menuName = "SpellConfigs/Fire/FireTotemConfig")]
public class FireTotemConfig : SpellConfig
{
    [Header("Totem")]
    public GameObject Totem;
    public GameObject Explosion;
    public float TimeBetweenAttacks;
    public float DeathFadeSpeed;

    public override void Cast(Transform source, Vector3 direction)
    {
        // adjust spawn position around player
        var instance = Instantiate(Totem, source.position, Quaternion.identity);
        instance.GetComponent<FireTotem>().Initialize(this);
    }
}
