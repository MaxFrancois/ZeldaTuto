using UnityEngine;

public class FireballInstance : MonoBehaviour
{
    private float pushTime;
    private float pushForce;
    private float damage;
    private Rigidbody2D body;
    private float lifeTimeTracker;
    private bool isDestroyed = false;
    private GameObject projectileParticles;
    private GameObject explosionParticles;
    private Vector3 direction;
    private float moveSpeed;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public void Initialize(float pushf, float pusht, float dmg, float movesp, float lifeTime, GameObject projectile, GameObject explosion, Vector3 dir)
    {
        pushForce = pushf;
        pushTime = pusht;
        damage = dmg;
        explosionParticles = explosion;
        projectileParticles = projectile;
        direction = dir;
        moveSpeed = movesp;
        lifeTimeTracker = lifeTime;
    }

    void Start()
    {
        //projectileParticles = Instantiate(projectileParticles, transform.position, transform.rotation) as GameObject;
        //projectileParticles.transform.parent = transform;
        body.AddForce(transform.position + direction.normalized * moveSpeed);
    }

    void FixedUpdate()
    {
        if (!isDestroyed)
        {
            lifeTimeTracker -= Time.deltaTime;
            if (lifeTimeTracker <= 0)
            {
                DestroyThis();
            }
            else UpdatePosition();
        }
    }

    private void UpdatePosition()
    {
        //body.MovePosition(transform.position + direction.normalized * moveSpeed * Time.deltaTime);
        //if (fireEffectRigidBody != null)
        //    fireEffectRigidBody.MovePosition(transform.position + fireDirection.normalized * MoveSpeed * Time.deltaTime);
    }

    private void DestroyThis()
    {
        if (!isDestroyed)
        {
            isDestroyed = true;
            Debug.Log("destroying fireball");
            //Destroy(AnimationInstance);
            var exp = Instantiate(explosionParticles, transform.position, Quaternion.identity);
            Destroy(exp, 3f);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        //update this to use knockback script instead
        if (!isDestroyed)
            if ((collidedObject.gameObject.CompareTag("Enemy") || collidedObject.gameObject.CompareTag("MiniBoss"))
                && collidedObject.isTrigger)
            {
                collidedObject.GetComponent<EnemyBase>().Knock(transform, pushTime, pushForce, damage);
                DestroyThis();
            }
            else if (collidedObject.gameObject.CompareTag("Breakable") || collidedObject.gameObject.CompareTag("WorldCollision"))
            {
                DestroyThis();
            }
    }
}
