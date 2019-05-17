using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpooderNet : ITime
{
    public float LifeTime;
    public float MoveSpeed;
    float remainingLifeTime;
    Vector2 target = Vector2.zero;
    bool moving = true;
    Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        remainingLifeTime = LifeTime;

    }
    public void Init(Vector2 targetPosition)
    {
        target = targetPosition;
    }

    void Update()
    {
        remainingLifeTime -= Time.deltaTime;
        if (remainingLifeTime <= 0)
            Destroy(gameObject);
        if (moving && target != Vector2.zero)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, MoveSpeed * Time.deltaTime * (1 - SlowTimeCoefficient));
        }
    }

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (collidedObject.gameObject.CompareTag("Player") && collidedObject.isTrigger)
        {
            StartCoroutine(StopMoving());
            collidedObject.GetComponentInParent<PlayerMovement>().SetMoveSpeed(0.5f);
        }
        if (collidedObject.gameObject.CompareTag("WorldCollision"))
        {
            moving = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collidedObject)
    {
        if (collidedObject.gameObject.CompareTag("Player") && collidedObject.isTrigger)
        {
            collidedObject.GetComponentInParent<PlayerMovement>().SetMoveSpeed(-1);
        }
    }

    IEnumerator StopMoving()
    {
        yield return new WaitForSeconds(0.2f);
        moving = false;
    }
}
