using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spooder : Enemy
{
    public SpooderNet SpooderNet;
    public float DashSpeed;
    public float TimeBetweenAttacks;
    private float timeSinceLastAttack;

    protected override void Update()
    {
        base.Update();
        if (!IsDead && CanAct())
        {
            if (TargetInChasingRange)
            {
                var r = Random.Range(0, 300);
                if (r != 0)
                {
                    var temp  = Vector3.MoveTowards(transform.position, target.position, MoveSpeed * Time.deltaTime * (1 - SlowTimeCoefficient));
                    ChangeMovementDirection(temp - transform.position);
                    transform.position = temp;
                    EnemyState.MovementState = CharacterMovementState.Walking;
                    animator.SetBool("IsWalking", true);
                }
                else
                {
                    StartCoroutine(ThrowNet());
                }
            }
            else if (TargetInAttackRange)
            {
                if (timeSinceLastAttack > 0)
                    timeSinceLastAttack -= Time.deltaTime - (1 * SlowTimeCoefficient);
                if (timeSinceLastAttack <= 0)
                    StartCoroutine(Attack());
            }
            else if (TargetOutOfRange)
            {
                EnemyState.MovementState = CharacterMovementState.Idle;
                animator.SetBool("IsWalking", false);
                timeSinceLastAttack = TimeBetweenAttacks;
            }
        }
        //else
        //{
        //    if (!IsDead)
        //    {
        //        animator.SetBool("IsWalking", false);
        //        timeSinceLastAttack = TimeBetweenAttacks;
        //    }
        //}
    }

    private IEnumerator ThrowNet()
    {
        animator.SetBool("IsWalking", false);
        EnemyState.MovementState = CharacterMovementState.Attacking;
        animator.SetTrigger("ThrowNet");
        var targetPosition = new Vector2(target.position.x, target.position.y);
        yield return new WaitForSeconds(1.2f);
        var net = Instantiate(SpooderNet, transform.position, Quaternion.identity);
        net.Init(targetPosition);
        yield return new WaitForSeconds(0.4f);
        EnemyState.MovementState = CharacterMovementState.Idle;
    }

    private IEnumerator Attack()
    {
        animator.SetBool("IsWalking", false);
        body.velocity = Vector2.zero;
        EnemyState.MovementState = CharacterMovementState.Attacking;
        animator.SetTrigger("Attack");
        Vector2 difference = target.transform.position - transform.position;
        yield return new WaitForSeconds(1.2f);
        difference = difference.normalized * DashSpeed * (1 - SlowTimeCoefficient);
        body.AddForce(difference, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.4f);
        body.velocity = Vector2.zero;
        EnemyState.MovementState = CharacterMovementState.Idle;
        timeSinceLastAttack = TimeBetweenAttacks;
    }

    private IEnumerator Jump()
    {
        EnemyState.MovementState = CharacterMovementState.Attacking;
        yield return null;
    }
}
