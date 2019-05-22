using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonoBehaviour
{
    private float pushTime;
    private float pushForce;
    private float damage;
    private float fallDistance;
    private CircleCollider2D circleCollider;
    private Rigidbody2D body;
    private bool isFalling;
    private float timeBeforeFall;
    private float timeBeforeFallTracker;
    private bool isDestroyed = false;
    private Vector2 spawnPosition;
    private SpriteRenderer sprite;
    private float startFadeTime = 0f;
    private float fadeSpeed;

    void Awake()
    {
        timeBeforeFallTracker = timeBeforeFall;
        isFalling = false;
        spawnPosition = transform.position;
        circleCollider = GetComponent<CircleCollider2D>();
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        circleCollider.enabled = false;
    }

    public void Initialize(Vector2 spawnPos, float pushf, float pusht, float dmg, float falldist, float timeb4fall, float fade)
    {
        spawnPosition = spawnPos;
        pushForce = pushf;
        pushTime = pusht;
        damage = dmg;
        fallDistance = falldist;
        timeBeforeFall = timeb4fall;
        timeBeforeFallTracker = timeb4fall;
        fadeSpeed = fade;
    }

    void FixedUpdate()
    {
        if (!isDestroyed)
        {
            if (!isFalling)
            {
                timeBeforeFallTracker -= Time.deltaTime;
                if (timeBeforeFallTracker <= 0)
                {
                    isFalling = true;
                    body.isKinematic = false;
                }
            }
            if (transform.position.y - 0.7f <= spawnPosition.y - fallDistance)
            {
                body.gravityScale = 0f;
                body.velocity = Vector2.zero;
                circleCollider.enabled = true;
                StartCoroutine(SetIsDestroyed());
            }
        }
        else
        {
            if (startFadeTime == 0f)
                startFadeTime = Time.time;
            float t = (Time.time - startFadeTime) / fadeSpeed;
            sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1f, 0f, t));
            if (sprite.color.a == 0f)
                Destroy(this.gameObject);
        }
    }

    //needed for the last second collision check
    private IEnumerator SetIsDestroyed()
    {
        yield return new WaitForSeconds(0.1f);
        isDestroyed = true;
    }

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (!collidedObject.CompareTag("Room"))
        {
            if (!isDestroyed && circleCollider.enabled)
                if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
                {
                    collidedObject.GetComponent<PlayerMovement>().TakeDamage(transform, pushTime, pushForce, damage);
                }
        }
    }
}
