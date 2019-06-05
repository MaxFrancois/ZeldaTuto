using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLog : Log
{
    public GameObject Projectile;
    public float FireDelay;
    private float fireDelayTracker;
    private bool canFire = true;

    protected override void Update()
    {
        fireDelayTracker -= Time.deltaTime;
        if (fireDelayTracker <= 0)
        {
            fireDelayTracker = FireDelay;
            canFire = true;
        }
    }

    void FixedUpdate()
    {
        if (CanAct())
        {
            if (TargetInChasingRange && canFire)
            {
                canFire = false;
                Vector3 tempVector = target.transform.position - transform.position;
                var projectile = Instantiate(Projectile, transform.position, Quaternion.identity);
                projectile.GetComponent<Projectile>().Launch(tempVector);
                animator.SetBool("IsAwake", true);
            }
            else if (TargetOutOfRange)
            {
                EnemyState.MovementState = CharacterMovementState.Idle;
                animator.SetBool("IsAwake", false);
            }
        }
    }
}
