using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : Enemy
{
    void FixedUpdate()
    {
        if (CanAct())
        {
            if (TargetInChasingRange)
            {
                var temp = Vector3.MoveTowards(transform.position, target.position, MoveSpeed * Time.deltaTime * (1 - SlowTimeCoefficient));
                ChangeMovementDirection(temp - transform.position);
                body.MovePosition(temp);
                EnemyState.MovementState = CharacterMovementState.Walking;
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
