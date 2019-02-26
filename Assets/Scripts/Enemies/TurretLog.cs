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

    public override void CheckRange()
    {
        if (Vector3.Distance(Target.position, transform.position) <= ChaseRadius
            && Vector3.Distance(Target.position, transform.position) > AttackRadius
            && CurrentState != EnemyState.Staggered && canFire)
        {
            canFire = false;
            Vector3 tempVector = Target.transform.position - transform.position;
            var projectile = Instantiate(Projectile, transform.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().Launch(tempVector);
            Animator.SetBool("IsAwake", true);
        }
        else if (Vector3.Distance(Target.position, transform.position) > ChaseRadius)
        {
            ChangeState(EnemyState.Idle);
            Animator.SetBool("IsAwake", false);
        }
    }
}
