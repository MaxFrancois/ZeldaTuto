using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Spell
{
    

    //private Rigidbody2D rigidBody;
    //private Rigidbody2D fireEffectRigidBody;
    //Vector3 fireDirection;
    //Transform sourceTransform;
    //public GameObject FireballExplosionAnimation;
    public FireballConfig Config;

    //private void Start()
    //{
    //    lifeTimeTracker = LifeTime;
    //    rigidBody = GetComponent<Rigidbody2D>();
    //}

    //void Update()
    //{
    //    if (LifeTime != 0)
    //    {
    //        lifeTimeTracker -= Time.deltaTime;
    //        if (lifeTimeTracker <= 0)
    //        {
    //            DestroyThis();
    //        }
    //        else UpdatePosition();
    //    }
    //    else
    //        UpdatePosition();
    //}

    //private void DestroyThis()
    //{
    //    if (!isDestroyed)
    //    {
    //        isDestroyed = true;
    //        Debug.Log("destroying fireball");
    //        Destroy(AnimationInstance);
    //        var explosion = Instantiate(FireballExplosionAnimation, transform.position, Quaternion.identity);
    //        Destroy(explosion, 0.5f);
    //        Destroy(this.gameObject, 0.5f);
    //    }
    //}

    //private void UpdatePosition()
    //{
    //    rigidBody.MovePosition(transform.position + fireDirection.normalized * MoveSpeed * Time.deltaTime);
    //    if (fireEffectRigidBody != null)
    //        fireEffectRigidBody.MovePosition(transform.position + fireDirection.normalized * MoveSpeed * Time.deltaTime);
    //}

    public override void Cast(Transform source, Vector3 direction)
    {
        var currentPosition = source.position;
        var instance = Instantiate(Config.FireballInstance, currentPosition, Quaternion.identity);
        var script = instance.GetComponent<FireballInstance>();
        var dir = new Vector2(direction.x, direction.y);
        //fireEffect.transform.rotation = Quaternion.Euler(dir);
        script.Initialize(Config.PushForce, Config.PushTime, Config.Damage, Config.MoveSpeed, Config.LifeTime, Config.FireballExplosion, dir);
        instance.transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(direction));
        Destroy(this.gameObject);
    }

    //private void OnTriggerEnter2D(Collider2D collidedObject)
    //{
    //    //update this to use knockback script instead
    //    if (!isDestroyed)
    //        if ((collidedObject.gameObject.CompareTag("Enemy") || collidedObject.gameObject.CompareTag("MiniBoss")) 
    //            && collidedObject.isTrigger)
    //        {
    //            collidedObject.GetComponent<EnemyBase>().Knock(transform, PushTime, PushForce, Damage);
    //            DestroyThis();
    //        }
    //        else if (collidedObject.gameObject.CompareTag("Breakable") || collidedObject.gameObject.CompareTag("WorldCollision"))
    //        {
    //            DestroyThis();
    //        }
    //}
    public override SpellConfig GetConfig()
    {
        return Config;
    }
}
