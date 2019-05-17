using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEnemy : Log
{
    public Collider2D Boundary;

    void FixedUpdate()
    {
        if (Vector3.Distance(target.position, transform.position) <= ChaseRadius
           && Vector3.Distance(target.position, transform.position) > AttackRadius
           && CurrentState != EnemyState.Staggered
           && Boundary.bounds.Contains(target.transform.position))
        {
            var temp = Vector3.MoveTowards(transform.position, target.position, MoveSpeed * Time.deltaTime * (1 - SlowTimeCoefficient));
            ChangeMovementDirection(temp - transform.position);
            body.MovePosition(temp);
            ChangeState(EnemyState.Walking);
            animator.SetBool("IsAwake", true);
        }
        else if (Vector3.Distance(target.position, transform.position) > ChaseRadius
            || !Boundary.bounds.Contains(target.transform.position))
        {
            ChangeState(EnemyState.Idle);
            animator.SetBool("IsAwake", false);
        }
    }
}
