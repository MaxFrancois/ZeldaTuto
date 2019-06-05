using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisengageGas : ITime
{
    DisengageConfig config;
    float lifeTime;

    public void Initialize(DisengageConfig cfg)
    {
        config = cfg;
        lifeTime = cfg.GasDuration;
    }

    void Update()
    {
        if (lifeTime > 0)
            lifeTime -= Time.deltaTime * (1 - SlowTimeCoefficient);
        else
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.isTrigger)
        {
            var enemyState = collision.GetComponent<CharacterState>();
            if(enemyState)
            {
                new Poison(config.PoisonStatusConfig).OnStart(collision.gameObject);
                //apply gas debuff
            }
        }
    }
}
