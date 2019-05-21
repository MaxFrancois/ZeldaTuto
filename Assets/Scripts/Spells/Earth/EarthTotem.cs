using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthTotem : ITime
{
    EarthTotemConfig config;
    float currentLifeTime;
    float startFadeTime;
    float timeSinceLastAttack;
    bool isDead;
    Animator anim;
    SpriteRenderer spriteRenderer;
    GameObject healingCircle;
    void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(EarthTotemConfig cfg)
    {
        config = cfg;
        currentLifeTime = config.LifeTime;
        var spawnPosition = transform.position;
        spawnPosition.y -= 0.5f;
        healingCircle = Instantiate(config.Circle, spawnPosition, Quaternion.identity);
        healingCircle.transform.Rotate(new Vector3(45, 0, 0));
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
            {
                Destroy(healingCircle);
                Destroy(gameObject);
            }
        }
    }

    IEnumerator Attack()
    {
        anim.SetTrigger("Cast");
        yield return null;
        //yield return new WaitForSeconds(0.3f);
        //Instantiate(config.Explosion, transform.position, Quaternion.identity);
    }
}
