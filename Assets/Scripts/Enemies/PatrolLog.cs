using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolLog : Log
{
    public Transform[] Path;
    public int currentPoint;
    public Transform currentGoal;
    public float RoundingDistance;
    
    public override void CheckRange()
    {
        Animator.SetBool("IsAwake", true);

        if (Vector3.Distance(Target.position, transform.position) <= ChaseRadius
            && Vector3.Distance(Target.position, transform.position) > AttackRadius
            && CurrentState != EnemyState.Staggered)
        {
            var temp = Vector3.MoveTowards(transform.position, Target.position, MoveSpeed * Time.deltaTime);
            ChangeAnim(temp - transform.position);
            body.MovePosition(temp);
        }
        else if (Vector3.Distance(Target.position, transform.position) > ChaseRadius)
        {
            if (Vector3.Distance(transform.position, currentGoal.position) > RoundingDistance)
            {
                var temp = Vector3.MoveTowards(transform.position, currentGoal.position, MoveSpeed * Time.deltaTime);
                ChangeAnim(temp - transform.position);
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
