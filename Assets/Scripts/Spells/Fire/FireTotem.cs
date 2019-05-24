using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTotem : ITime
{
    FireTotemConfig config;
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

    public void Initialize(FireTotemConfig cfg)
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
            if (timeSinceLastAttack >= config.TimeBetweenAttacks && !isDead)
            {
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
        yield return new WaitForSeconds(0.3f);
        var spawnPosition = transform.position;
        spawnPosition.y -= 0.5f;
        var explosion = Instantiate(config.Explosion, spawnPosition, Quaternion.identity);
        explosion.GetComponent<FireTotemExplosion>().Initialize(config);
        explosion.transform.Rotate(new Vector3(45, 0, 0));
        yield return new WaitForSeconds(0.2f);
        explosion.GetComponent<CircleCollider2D>().enabled = false;
        Destroy(explosion, 2f);
    }
}
