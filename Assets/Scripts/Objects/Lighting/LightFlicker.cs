using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private float flickerTime;
    public float MinFlickerTime;
    public float MaxFlickerTime;
    public float SizeDifference;
    private bool isBig = false;
    public bool IsProgressiveFlickering;
    private SpriteMask mask;

    public float MiddleScaleX;
    public float MiddleScaleY;
    public float FinalScaleX;
    public float FinalScaleY;
    public float Speed;
    private bool gettingBigger = true;


    private void Start()
    {
        mask = GetComponent<SpriteMask>();
    }

    void Update()
    {
        if (mask.enabled)
        {
            flickerTime -= Time.deltaTime;
            if (!IsProgressiveFlickering)
            {
                if (flickerTime <= 0)
                {
                    flickerTime = Random.Range(MinFlickerTime, MaxFlickerTime);
                    if (isBig)
                    {
                        isBig = false;
                        transform.localScale = new Vector3(transform.localScale.x - SizeDifference, transform.localScale.y - SizeDifference, transform.localScale.z);
                    }
                    else
                    {
                        isBig = true;
                        transform.localScale = new Vector3(transform.localScale.x + SizeDifference, transform.localScale.y + SizeDifference, transform.localScale.z);
                    }
                }
            }
            else
            {
                if (gettingBigger)
                {
                    transform.localScale = new Vector3(
                        Mathf.MoveTowards(transform.localScale.x, MiddleScaleX, Speed * Time.deltaTime),
                        Mathf.MoveTowards(transform.localScale.y, MiddleScaleY, Speed * Time.deltaTime),
                        transform.localScale.z);
                    if (transform.localScale.x >= MiddleScaleX && transform.localScale.y >= MiddleScaleY)
                        gettingBigger = false;
                }
                else
                {
                    transform.localScale = new Vector3(
                        Mathf.MoveTowards(transform.localScale.x, FinalScaleX, Speed * Time.deltaTime),
                        Mathf.MoveTowards(transform.localScale.y, FinalScaleY, Speed * Time.deltaTime),
                        transform.localScale.z);
                }
            }
        }
    }
}
