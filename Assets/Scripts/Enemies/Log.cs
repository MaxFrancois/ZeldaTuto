using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : Enemy
{
    void FixedUpdate()
    {
        if (TargetInChasingRange && CurrentState != EnemyState.Staggered)
        {
            var temp = Vector3.MoveTowards(transform.position, target.position, MoveSpeed * Time.deltaTime * (1 - SlowTimeCoefficient));
            ChangeMovementDirection(temp - transform.position);
            body.MovePosition(temp);
            ChangeState(EnemyState.Walking);
            animator.SetBool("IsAwake", true);
        }
        else if (TargetOutOfRange)
        {
            ChangeState(EnemyState.Idle);
            animator.SetBool("IsAwake", false);
        }
    }
}
