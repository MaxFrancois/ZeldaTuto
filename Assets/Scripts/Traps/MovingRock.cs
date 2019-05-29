using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class MovingRock : MonoBehaviour
{
    public float MoveSpeed;
    public float PauseTime;
    public Direction StartDirection;
    Direction currentDirection;
    Rigidbody2D body;
    Animator animator;
    bool isMoving;
    
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentDirection = StartDirection;
        isMoving = true;
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            body.velocity = GetCurrentDirection() * MoveSpeed * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WorldCollision"))
        {
            StartCoroutine(ChangeDirection());
        }
    }

    IEnumerator ChangeDirection()
    {
        isMoving = false;
        body.velocity = Vector2.zero;
        animator.SetBool(currentDirection.ToString(), false);
        yield return new WaitForSeconds(PauseTime / 2);
        ReverseDirection();
        yield return new WaitForSeconds(PauseTime / 2);
        animator.SetBool(currentDirection.ToString(), true);
        isMoving = true;
    }

    Vector2 GetCurrentDirection()
    {
        int x = 0;
        int y = 0;
        if (currentDirection == Direction.Up)
            y = 1;
        if (currentDirection == Direction.Down)
            y = -1;
        if (currentDirection == Direction.Left)
            x = -1;
        if (currentDirection == Direction.Right)
            x = 1;
        return new Vector2(x, y);
    }

    void ReverseDirection()
    {
        if (currentDirection == Direction.Up)
        {
            currentDirection = Direction.Down;
            return;
        }
        if (currentDirection == Direction.Down)
        {
            currentDirection = Direction.Up;
            return;
        }
        if (currentDirection == Direction.Left)
        {
            currentDirection = Direction.Right;
            return;
        }
        if (currentDirection == Direction.Right)
        {
            currentDirection = Direction.Left;
            return;
        }
    }
}