using UnityEngine;

public class FireTotemExplosion : MonoBehaviour
{
    FireTotemConfig config;
    public void Initialize(FireTotemConfig cfg)
    {
        config = cfg;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.isTrigger)
        {
            collision.GetComponent<EnemyBase>().TakeDamage(transform, config.PushTime, config.PushForce, config.Damage);
        }
    }
}
