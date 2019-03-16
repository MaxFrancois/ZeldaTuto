using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : Enemy
{
    public float ChaseRadius;
    public float AttackRadius;
    public Animator Animator;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        Animator  = GetComponent<Animator>();
        Target = GameObject.FindWithTag("Player").transform;
        CurrentState = EnemyState.Idle;
        Animator.SetBool("IsAwake", true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckRange();
    }

    public virtual void CheckRange()
    {
        if (Vector3.Distance(Target.position, transform.position) <= ChaseRadius
            && Vector3.Distance(Target.position, transform.position) > AttackRadius
            && CurrentState != EnemyState.Staggered)
        {
            var temp = Vector3.MoveTowards(transform.position, Target.position, MoveSpeed * Time.deltaTime);
            ChangeAnim(temp - transform.position);
            body.MovePosition(temp);
            ChangeState(EnemyState.Walking);
            Animator.SetBool("IsAwake", true);
        }
        else if (Vector3.Distance(Target.position, transform.position) > ChaseRadius)
        {
            ChangeState(EnemyState.Idle);
            Animator.SetBool("IsAwake", false);
        }
    }

    public void ChangeAnim(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0) { SetAnimFloat(Vector2.right); }
            else if (direction.x < 0) { SetAnimFloat(Vector2.left); }
        }
        else if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if (direction.y > 0) { SetAnimFloat(Vector2.up); }
            else if (direction.y < 0) { SetAnimFloat(Vector2.down); }
        }
    }

    public void SetAnimFloat(Vector2 setVector)
    {
        Animator.SetFloat("MoveX", setVector.x);
        Animator.SetFloat("MoveY", setVector.y);
    }

    protected void ChangeState(EnemyState state)
    {
        if (state != CurrentState)
            CurrentState = state;
    }
}
