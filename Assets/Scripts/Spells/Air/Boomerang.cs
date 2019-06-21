using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang: ITime
{
    BoomerangConfig config;
    Vector3 direction;
    bool goingForward = true;
    Rigidbody2D body;
    float currentLifeTime;
    GameObject spellCaster;
    float timeBeforeReturn;

    public void Initialize(BoomerangConfig cfg, Vector3 dir, GameObject caster)
    {
        config = cfg;
        direction = dir;
        currentLifeTime = cfg.ThrowDuration;
        spellCaster = caster;
        timeBeforeReturn = config.TimeBeforeReturn;
    }

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (goingForward)
        {
            currentLifeTime -= Time.fixedDeltaTime * (1 - SlowTimeCoefficient);
            if (currentLifeTime > 0)
                body.velocity = new Vector2(direction.x, direction.y).normalized * config.ProjectileSpeed * Time.fixedDeltaTime * (1 - SlowTimeCoefficient);
            else
            {
                body.velocity = Vector2.zero;
                timeBeforeReturn -= Time.deltaTime * (1 - SlowTimeCoefficient);
                if (timeBeforeReturn <= 0)
                {
                    goingForward = false;
                }
            }
        }
        else
        {
            var directionToPlayer = Vector3.MoveTowards(transform.position, spellCaster.transform.position, config.ProjectileReturnSpeed * Time.deltaTime * (1 - SlowTimeCoefficient));
            transform.position = directionToPlayer;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.gameObject == spellCaster && !goingForward)
        {
            Destroy(gameObject);
        }
        if (collision.CompareTag("Enemy") && collision.isTrigger)
        {
            var enemy = collision.GetComponent<EnemyBase>();
            enemy.TakeDamage(transform, config.PushTime, config.PushForce, config.Damage);
        }
    }
}
