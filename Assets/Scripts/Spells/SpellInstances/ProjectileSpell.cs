using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpell : Spell
{
    public float MoveSpeed;
    public float Distance;

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        Destroy(this.gameObject);
    }
}
