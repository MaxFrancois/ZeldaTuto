using UnityEngine;

public class Dummy : Enemy
{
    public float ResetAfterTime;
    float timeSinceLastHit;
    bool inCombat;
    public GameObject ResetAnimation;

    void Start()
    {
        inCombat = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (inCombat)
        {
            timeSinceLastHit += Time.deltaTime;
            if (timeSinceLastHit > ResetAfterTime)
                ResetPosition();
        }
    }

    void ResetPosition()
    {
        var portal1 = Instantiate(ResetAnimation, transform.position, Quaternion.identity);
        var portal2 = Instantiate(ResetAnimation, HomePosition, Quaternion.identity);
        Destroy(portal1, .3f);
        Destroy(portal2, .3f);
        inCombat = false;
        transform.position = HomePosition;
    }

    public override void TakeDamage(Transform thingThatHitYou, float pushTime, float pushForce, float damage, bool display = true)
    {
        CurrentState = EnemyState.Staggered;
        LoseHealth(damage, display);
        GainHealth(damage, false);

        Vector2 difference = transform.position - thingThatHitYou.position;
        difference = difference.normalized * pushForce * (1 - SlowTimeCoefficient);
        body.AddForce(difference, ForceMode2D.Impulse);
        if (transform.gameObject.activeInHierarchy)
            StartCoroutine(Knockback(body, pushTime));
        timeSinceLastHit = 0;
        inCombat = true;
    }
}
