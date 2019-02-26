using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float MoveSpeed;
    public float MoveDuration;
    private float moveDurationTracker;
    public Vector2 MoveDirection;
    public Rigidbody2D rigidBody;
    
    void Start()
    {
        moveDurationTracker = MoveDuration;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        moveDurationTracker -= Time.deltaTime;
        if (moveDurationTracker <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void Launch(Vector2 velocity)
    {
        rigidBody.velocity = velocity * MoveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        Destroy(this.gameObject);
    }
}
