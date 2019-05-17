using System.Collections;
using System.Collections.Generic;
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
    void Update()
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

    public override void Knock(Transform thingThatHitYou, float pushTime, float pushForce, float damage)
    {
        base.Knock(thingThatHitYou, pushTime, pushForce, 0);
        timeSinceLastHit = 0;
        inCombat = true;
    }
}
