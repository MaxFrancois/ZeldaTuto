using UnityEngine;

[CreateAssetMenu(fileName = "DarkTotemConfig", menuName = "SpellConfigs/Dark/DarkTotemConfig")]
public class DarkTotemConfig : SpellConfig
{
    [Header("Totem")]
    public GameObject Totem;
    public float AttackRadius;
    public float TimeBetweenAttacks;
    public float DeathFadeSpeed;
    [Header("Projectiles")]
    public GameObject Projectile;
    public float AmountOfProjectiles;
    public float AngleBetweenProjectiles;
    public float ProjectileSpeed;
    public float ProjectileLifeTime;

    public override void Cast(Transform source, Vector3 direction)
    {
        // adjust spawn position around player
        var instance = Instantiate(Totem, source.position, Quaternion.identity);
        instance.GetComponent<DarkTotem>().Initialize(this);
    }
}
