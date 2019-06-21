using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoomrangConfig", menuName = "SpellConfigs/Air/BoomrangConfig")]
public class BoomerangConfig : SpellConfig
{
    public float AmountOfProjectiles;
    public float AngleBetweenProjectiles;
    public float ProjectileSpeed;
    public float ProjectileReturnSpeed;
    public float ThrowDuration;
    public float TimeBeforeReturn;
    public GameObject Projectile;

    public override void Cast(Transform source, Vector3 direction)
    {
        for (int i = 0; i < AmountOfProjectiles; i++)
        {
            var idx = i - Mathf.Floor(AmountOfProjectiles / 2);
            var projectileInstance = Instantiate(Projectile, source.position, Quaternion.identity);
            var dir = Quaternion.AngleAxis(AngleBetweenProjectiles * idx, Vector3.back) * direction;
            projectileInstance.GetComponent<Boomerang>().Initialize(this, dir, source.gameObject);
        }
    }
}
