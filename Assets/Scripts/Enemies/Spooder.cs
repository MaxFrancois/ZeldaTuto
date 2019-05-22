using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spooder : Enemy
{
    public SpooderNet SpooderNet;
    public float DashSpeed;
    public float TimeBetweenAttacks;
    private float timeSinceLastAttack;

    void Update()
    {
        if (timeSinceLastAttack > 0)
            timeSinceLastAttack -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (!IsDead)
        {
            if (TargetInChasingRange && CurrentState != EnemyState.Staggered && CurrentState != EnemyState.Attacking)
            {
                var r = Random.Range(0, 300);
                if (r != 0)
                {
                    var temp = Vector2.MoveTowards(transform.position, target.position, MoveSpeed * Time.deltaTime * (1 - SlowTimeCoefficient));
                    ChangeMovementDirection(temp - (Vector2)transform.position);
                    //transform.position = temp;
                    ChangeState(EnemyState.Walking);
                    animator.SetBool("IsWalking", true);
                }
                else
                {
                    StartCoroutine(ThrowNet());
                }
            }
            else if (TargetInAttackRange && CurrentState != EnemyState.Attacking && CurrentState != EnemyState.Staggered)
            {
                if (timeSinceLastAttack <= 0)
                    StartCoroutine(Attack());
            }
            else if (TargetOutOfRange)
            {
                ChangeState(EnemyState.Idle);
                animator.SetBool("IsWalking", false);
            }
        }
    }

    private IEnumerator ThrowNet()
    {
        animator.SetBool("IsWalking", false);
        ChangeState(EnemyState.Attacking);
        animator.SetTrigger("ThrowNet");
        yield return new WaitForSeconds(1.2f);
        var net = Instantiate(SpooderNet, transform.position, Quaternion.identity);
        net.Init(new Vector2(target.position.x, target.position.y));
        yield return new WaitForSeconds(0.4f);
        ChangeState(EnemyState.Idle);
    }

    private IEnumerator Attack()
    {
        animator.SetBool("IsWalking", false);
        body.velocity = Vector2.zero;
        ChangeState(EnemyState.Attacking);
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1.2f);
        Vector2 difference = target.transform.position - transform.position;
        difference = difference.normalized * DashSpeed * (1 - SlowTimeCoefficient);
        body.AddForce(difference, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.4f);
        body.velocity = Vector2.zero;
        ChangeState(EnemyState.Idle);
        timeSinceLastAttack = TimeBetweenAttacks;
    }

    private IEnumerator Jump()
    {
        CurrentState = EnemyState.Attacking;
        yield return null;
    }
}
