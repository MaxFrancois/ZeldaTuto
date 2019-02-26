using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEnemy : Log
{
    public Collider2D Boundary;

    public override void CheckRange()
    {
        if (Vector3.Distance(Target.position, transform.position) <= ChaseRadius
           && Vector3.Distance(Target.position, transform.position) > AttackRadius
           && CurrentState != EnemyState.Staggered
           && Boundary.bounds.Contains(Target.transform.position))
        {
            var temp = Vector3.MoveTowards(transform.position, Target.position, MoveSpeed * Time.deltaTime);
            ChangeAnim(temp - transform.position);
            body.MovePosition(temp);
            ChangeState(EnemyState.Walking);
            Animator.SetBool("IsAwake", true);
        }
        else if (Vector3.Distance(Target.position, transform.position) > ChaseRadius
            || !Boundary.bounds.Contains(Target.transform.position))
        {
            ChangeState(EnemyState.Idle);
            Animator.SetBool("IsAwake", false);
        }
    }
}
