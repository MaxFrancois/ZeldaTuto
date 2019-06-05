using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disengage : ITime
{
    DisengageConfig config;
    float lifeTime;
    bool isDestroyed = false;
    List<EnemyBase> enemiesInRange;

    public void Initialize(DisengageConfig cfg)
    {
        config = cfg;
        lifeTime = cfg.LifeTime;
        enemiesInRange = new List<EnemyBase>();
    }

    void Update()
    {
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime * (1 - SlowTimeCoefficient);
        }
        else if (!isDestroyed)
        {
            isDestroyed = true;
            var explosion = Instantiate(config.ExplosionParticles, transform.position, Quaternion.identity);
            foreach (var enemy in enemiesInRange)
            {
                enemy.TakeDamage(transform, 0, 0, config.Damage, false);
                var state = enemy.GetComponent<CharacterState>();
                if (state)
                {
                    new Stun(config.StunStatusConfig).OnStart(enemy.gameObject);
                }
            }
            var gas = Instantiate(config.Gas, transform.position, Quaternion.identity);
            gas.Initialize(config);
            Destroy(explosion, 3f);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger && collision.gameObject.CompareTag("Enemy"))
        {
            var script = collision.GetComponent<EnemyBase>();
            if (script) enemiesInRange.Add(script);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.isTrigger && collision.gameObject.CompareTag("Enemy"))
        {
            var script = collision.GetComponent<EnemyBase>();
            if (script) enemiesInRange.Remove(script);
        }
    }
}
