using System.Collections.Generic;
using UnityEngine;

public class SlowTime : MonoBehaviour
{
    bool isGrowing, isShrinking = false;
    SlowTimeConfig config;
    float lifeTime;
    List<ITime> slowedObjects;
    
    public void Initialize(SlowTimeConfig cfg)
    {
        config = cfg;
        lifeTime = cfg.LifeTime;
        isGrowing = true;
        slowedObjects = new List<ITime>();
    }

    void Update()
    {
        if (isGrowing)
        {
            transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * config.ExpandSpeed, transform.localScale.y + Time.deltaTime * config.ExpandSpeed, 0);
            if (transform.localScale.x >= config.MaxScale)
                isGrowing = false;
        }
        if (!isGrowing && !isShrinking)
        {
            lifeTime -= Time.deltaTime;
        }

        if (lifeTime <= 0)
            isShrinking = true;

        if (isShrinking)
        {
            transform.localScale = new Vector3(transform.localScale.x - Time.deltaTime * config.ExpandSpeed, transform.localScale.y - Time.deltaTime * config.ExpandSpeed, 0);
            if (transform.localScale.x <= 0.2)
            {
                foreach (var so in slowedObjects)
                    so.SlowTimeCoefficient = 0;
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && collision.isTrigger)
        {
            var timeComponent = collision.GetComponent<ITime>();
            if (timeComponent)
            {
                timeComponent.SlowTimeCoefficient = config.SlowSpeed;
                slowedObjects.Add(timeComponent);
            }
            else
            {
                if (collision.transform.parent)
                {
                    var parentTime = collision.transform.parent.GetComponent<ITime>();
                    if (parentTime)
                    {
                        parentTime.SlowTimeCoefficient = config.SlowSpeed;
                        slowedObjects.Add(parentTime);
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && collision.isTrigger)
        {
            var timeComponent = collision.GetComponent<ITime>();
            if (timeComponent)
            {
                timeComponent.SlowTimeCoefficient = 0;
                slowedObjects.Remove(timeComponent);
            }
            else
            {
                if (collision.transform.parent)
                {
                    var parentTime = collision.transform.parent.GetComponent<ITime>();
                    if (parentTime)
                    {
                        parentTime.SlowTimeCoefficient = 0;
                        slowedObjects.Remove(parentTime);
                    }
                }
            }
        }
    }
}
