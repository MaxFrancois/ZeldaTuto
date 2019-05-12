using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolLog : Log
{
    public Transform[] Path;
    public int currentPoint;
    public Transform currentGoal;
    public float RoundingDistance;

    void FixedUpdate()
    {
        animator.SetBool("IsAwake", true);

        if (TargetInChasingRange && CurrentState != EnemyState.Staggered)
        {
            var temp = Vector3.MoveTowards(transform.position, target.position, MoveSpeed * Time.deltaTime);
            ChangeMovementDirection(temp - transform.position);
            body.MovePosition(temp);
        }
        else if (TargetOutOfRange)
        {
            if (Vector3.Distance(transform.position, currentGoal.position) > RoundingDistance)
            {
                var temp = Vector3.MoveTowards(transform.position, currentGoal.position, MoveSpeed * Time.deltaTime);
                ChangeMovementDirection(temp - transform.position);
                body.MovePosition(temp);
            } else
            {
                ChangeGoal();
            }
        }
    }

    private void ChangeGoal()
    {
        if (currentPoint == Path.Length - 1)
        {
            currentPoint = 0;
            currentGoal = Path[0];
        }
        else
        {
            currentPoint++;
            currentGoal = Path[currentPoint];
        }
    }
}
