using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpell : Spell
{
    public float MoveSpeed;
    public float Distance;

    public override void Cast(Transform source, Vector3 direction)
    {
        throw new System.NotImplementedException();
    }

    public override SpellConfig GetConfig()
    {
        throw new System.NotImplementedException();
    }

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        Destroy(this.gameObject);
    }
}
