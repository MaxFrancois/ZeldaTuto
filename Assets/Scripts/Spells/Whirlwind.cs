using UnityEngine;

public class Whirlwind : MonoBehaviour
{
    private float duration;
    private float damage;
    private float pullRadius;
    private float pullForce;

    public void Initialize(float dmg, float lifeTime, float radius, float force)
    {
        duration = lifeTime;
        damage = dmg;
        pullRadius = radius;
        pullForce = force;
    }

    void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, pullRadius))
            {
                if (collider.isTrigger && collider.gameObject.CompareTag("Enemy"))
                    collider.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, pullRadius))
        {
            if (collider.isTrigger && collider.gameObject.CompareTag("Enemy"))
            {
                // calculate direction from target to me
                Vector3 forceDirection = transform.position - collider.transform.position;
                // apply force on target towards me
                collider.GetComponent<Rigidbody2D>().AddForce(forceDirection.normalized * pullForce * Time.fixedDeltaTime);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.isTrigger && collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<EnemyBase>();
            enemy.Knock(transform, 0, 0, damage);
            Debug.Log("Whirlwind hitting " + enemy.Name + ", HP leftover: " + enemy.GetCurrentHealth());
        }
    }
}
