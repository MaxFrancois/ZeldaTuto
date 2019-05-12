using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireBallConfig", menuName = "SpellConfigs/FireBallConfig")]
public class FireballConfig : SpellConfig
{
    public GameObject FireballExplosion;
    public GameObject FireballInstance;
    public float MoveSpeed;

    public override void Cast(Transform source, Vector3 direction)
    {
        var currentPosition = source.position;
        var instance = Instantiate(FireballInstance, currentPosition, Quaternion.identity);
        var script = instance.GetComponent<FireballInstance>();
        var dir = new Vector2(direction.x, direction.y);
        //fireEffect.transform.rotation = Quaternion.Euler(dir);
        script.Initialize(PushForce, PushTime, Damage, MoveSpeed, LifeTime, FireballInstance, FireballExplosion, dir);
        instance.transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(direction));
    }
}
