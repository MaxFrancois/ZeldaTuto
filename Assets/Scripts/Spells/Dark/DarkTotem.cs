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
                //StartCoroutine(Die());
            }
            timeSinceLastAttack += Time.deltaTime * (1 - SlowTimeCoefficient);
            if (timeSinceLastAttack >= config.TimeBetweenAttacks && Utilities.FindClosestEnemyInRadius(transform, config.AttackRadius) != null && !isDead)
            {
                Debug.Log("Dark totem cast");
                timeSinceLastAttack = 0;
                StartCoroutine(Attack());
            }
        }
        else
        {
            if (Utilities.FadeOutSprite(spriteRenderer, config.DeathFadeSpeed, startFadeTime))
                Destroy(gameObject);
        }
    }

    IEnumerator Attack()
    {
        anim.SetTrigger("Cast");
        yield return new WaitForSeconds(0.5f);
        var closestEnemy = Utilities.FindClosestEnemyInRadius(transform, config.AttackRadius);
        var direction = closestEnemy.transform.position - transform.position;
        for (int i = 0; i < config.AmountOfProjectiles; i++)
        {
            var projectileInstance = Instantiate(config.Projectile, transform.position, Quaternion.identity);
            projectileInstance.GetComponent<DarkTotemProjectile>().Initialize(config, direction);
        }
        yield return null;
    }

    IEnumerator Die()
    {
        
        yield return null;
    }
}
