using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkTotemProjectile : ITime
{
    DarkTotemConfig config;
    Rigidbody2D body;
    Vector3 direction;
    float currentLifeTime;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();

    }

    public void Initialize(DarkTotemConfig cfg, Vector3 dir)
    {
        config = cfg;
        direction = dir;
        currentLifeTime = config.ProjectileLifeTime;
    }

    void FixedUpdate()
    {
        body.velocity = new Vector2(direction.x, direction.y) * config.ProjectileSpeed * Time.fixedDeltaTime * (1 - SlowTimeCoefficient);
        currentLifeTime -= Time.fixedDeltaTime * (1 - SlowTimeCoefficient);
        if (currentLifeTime < 0)
            DestroyThis();
    }

    void DestroyThis()
    {
        //particle
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.isTrigger)
        {
            var enemy = collision.GetComponent<EnemyBase>();
            enemy.TakeDamage(transform, config.PushTime, config.PushForce, config.Damage);
        }
        if (collision.CompareTag("WorldCollision"))
        {
            DestroyThis();
        }
    }
}
