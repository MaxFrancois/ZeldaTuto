using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : ProjectileSpell
{
    private Rigidbody2D rigidBody;
    private Rigidbody2D fireEffectRigidBody;
    Vector3 fireDirection;
    Transform sourceTransform;
    public GameObject FireballExplosionAnimation;

    private void Start()
    {
        lifeTimeTracker = LifeTime;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (LifeTime != 0)
        {
            lifeTimeTracker -= Time.deltaTime;
            if (lifeTimeTracker <= 0)
            {
                DestroyThis();
            }
            else UpdatePosition();
        }
        else
            UpdatePosition();
    }

    private void DestroyThis()
    {
        if (!isDestroyed)
        {
            isDestroyed = true;
            Debug.Log("destroying fireball");
            Destroy(AnimationInstance);
            var explosion = Instantiate(FireballExplosionAnimation, transform.position, Quaternion.identity);
            Destroy(explosion, 0.5f);
            Destroy(this.gameObject, 0.5f);
        }
    }

    private void UpdatePosition()
    {
        cooldownTracker -= Time.deltaTime;
        rigidBody.MovePosition(transform.position + fireDirection.normalized * MoveSpeed * Time.deltaTime);
        if (fireEffectRigidBody != null)
            fireEffectRigidBody.MovePosition(transform.position + fireDirection.normalized * MoveSpeed * Time.deltaTime);
    }

    public override void Cast(Transform source, Vector3 direction)
    {
        fireDirection = direction;
        sourceTransform = source;
        StartCoroutine(FireCo(source, direction, Distance));
    }

    private IEnumerator FireCo(Transform source, Vector3 direction, float distance)
    {
        var currentPosition = source.position;
        AnimationInstance = Instantiate(Animation, currentPosition, Quaternion.identity);
        fireEffectRigidBody = AnimationInstance.GetComponent<Rigidbody2D>();
        var dir = new Vector2(direction.x, direction.y);
        //fireEffect.transform.rotation = Quaternion.Euler(dir);
        AnimationInstance.transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(direction));
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (!isDestroyed)
            if ((collidedObject.gameObject.CompareTag("Enemy") || collidedObject.gameObject.CompareTag("MiniBoss")) 
                && collidedObject.isTrigger)
            {
                Vector2 difference = collidedObject.transform.position - transform.position;
                difference = difference.normalized * PushForce;
                var collidedBody = collidedObject.GetComponent<Rigidbody2D>();
                collidedBody.AddForce(difference, ForceMode2D.Impulse);
                collidedBody.GetComponent<Enemy>().CurrentState = EnemyState.Staggered;
                collidedObject.GetComponent<Enemy>().Knock(collidedBody, PushTime, Damage);
                DestroyThis();
            }
            else if (collidedObject.gameObject.CompareTag("Breakable"))
            {
                Debug.Log("Fireball hit a breakable");
                DestroyThis();
            }
            else if (collidedObject.gameObject.CompareTag("WorldCollision"))
            {
                Debug.Log("Fireball hit a wall");
                DestroyThis();
            }
    }
}
