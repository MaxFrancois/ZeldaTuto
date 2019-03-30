using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherTrigger : MonoBehaviour
{
    public WeatherType WeatherType;
    public FloatSignal WeatherSignal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.isTrigger)
            WeatherSignal.Raise((float)WeatherType);
    }
}
