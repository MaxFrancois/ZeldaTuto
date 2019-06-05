using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEEffect : ITime
{
    public GameObject zone;
    private Vector3 maxZoneSize;
    private float expandSpeed;
    private float fadeSpeed;
    private SpriteRenderer circleSprite;
    private SpriteRenderer zoneSprite;
    private float startFadeTime = 0f;

    void Awake()
    {
        zone.transform.localScale = new Vector3(1, 1, 0);
        circleSprite = GetComponent<SpriteRenderer>();
        zoneSprite = zone.GetComponent<SpriteRenderer>();
    }

    public void Initialize(float expandspeed, Vector3 circleSize, Vector3 maxzonesize, float fadespeed)
    {
        transform.localScale = circleSize;
        maxZoneSize = maxzonesize;
        expandSpeed = expandspeed;
        fadeSpeed = fadespeed;
    }

    void Update()
    {
        if (zone.transform.localScale.x < maxZoneSize.x) {
            zone.transform.localScale = new Vector3(zone.transform.localScale.x + Time.deltaTime * expandSpeed * (1 - SlowTimeCoefficient), zone.transform.localScale.y + Time.deltaTime * expandSpeed * (1 - SlowTimeCoefficient), 0);
        }
        else
        {
            if (startFadeTime == 0f)
                startFadeTime = Time.time;
            float t = (Time.time - startFadeTime) / fadeSpeed;
            circleSprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1f, 0f, t));
            zoneSprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1f, 0f, t));
            if (circleSprite.color.a == 0f)
                Destroy(this.gameObject, 0.1f);
        }
    }
}
