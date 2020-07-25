using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkTotem : ITime
{
    DarkTotemConfig config;
    float currentLifeTime;
    float startFadeTime;
    float timeSinceLastAttack;
    bool isDead;
    Animator anim;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(DarkTotemConfig cfg)
    {
        config = cfg;
        currentLifeTime = config.LifeTime;
    }

    void Update()
    {
        if (!isDead)
        {
            if (currentLifeTime > 0) currentLifeTime -= Time.deltaTime * (1 - SlowTimeCoefficient);
            if (currentLifeTime <= 0)
            {
                startFadeTime = Time.time;
                isDead = true;
                anim.SetTrigger("Die");
                anim.SetBool("IsDead", true);
            }
            timeSinceLastAttack += Time.deltaTime * (1 - SlowTimeCoefficient);
            if (timeSinceLastAttack >= config.TimeBetweenAttacks && MyUtilities.FindClosestEnemyInRadius(transform, config.AttackRadius) != null && !isDead)
            {
                timeSinceLastAttack = 0;
                StartCoroutine(Attack());
            }
        }
        else
        {
            if (MyUtilities.FadeOutSprite(spriteRenderer, config.DeathFadeSpeed, startFadeTime))
                Destroy(gameObject);
        }
    }

    IEnumerator Attack()
    {
        anim.SetTrigger("Cast");
        yield return new WaitForSeconds(0.3f);
        var closestEnemy = MyUtilities.FindClosestEnemyInRadius(transform, config.AttackRadius);
        var enemyDirection = closestEnemy.transform.position - transform.position;
        for (int i = 0; i < config.AmountOfProjectiles; i++)
        {
            var idx = i - Mathf.Floor(config.AmountOfProjectiles / 2);
            var direction = enemyDirection;
            var projectileInstance = Instantiate(config.Projectile, transform.position, Quaternion.identity);
            direction = Quaternion.AngleAxis(config.AngleBetweenProjectiles * idx, Vector3.back) * direction;
            projectileInstance.GetComponent<DarkTotemProjectile>().Initialize(config, direction);
        }
        yield return null;
    }
}
