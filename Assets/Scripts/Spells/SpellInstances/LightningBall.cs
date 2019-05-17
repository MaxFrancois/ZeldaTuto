using UnityEngine;

public class LightningBall : MonoBehaviour
{
    LightningBallsConfig config;
    float lifeTime;

    public void Initialize(LightningBallsConfig cfg)
    {
        config = cfg;
        lifeTime = cfg.LifeTime;
    }

    void Update()
    {
        if (lifeTime > 0)
            lifeTime -= Time.deltaTime;
        else
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.isTrigger)
        {
            collision.GetComponent<Enemy>().Knock(transform, config.PushTime, config.PushForce, config.Damage);
            var explosion = Instantiate(config.LightningBallExplosion, transform.position, Quaternion.identity);
            Destroy(explosion, 2f);
            Destroy(gameObject);
        }
    }
}
