using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLog : Log
{
    public GameObject Projectile;
    public float FireDelay;
    private float fireDelayTracker;
    private bool canFire = true;

    private void Update()
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
        if (TargetInChasingRange && CurrentState != EnemyState.Staggered && canFire)
        {
            canFire = false;
            Vector3 tempVector = target.transform.position - transform.position;
            var projectile = Instantiate(Projectile, transform.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().Launch(tempVector);
            animator.SetBool("IsAwake", true);
        }
        else if (TargetOutOfRange)
        {
            ChangeState(EnemyState.Idle);
            animator.SetBool("IsAwake", false);
        }
    }
}
