using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum WeatherType
{
    None,
    LightRain,
    HeavyRain,
    Snow
}

public enum WeatherAction
{
    None,
    Increase,
    Decrease
}

[System.Serializable]
public class Weather
{
    public string Name;
    public ParticleSystem Particles;
    public WeatherType Type;
    public WeatherAction Action;
    public float MaxIntensity;
    public float IncreaseIntensitySpeed;
    public float DecreaseIntensitySpeed;
}

public class WeatherManager : MonoBehaviour
{
    private WeatherType currentWeather;
    public List<Weather> WeatherOptions;
    public GameObject FollowTarget;
    private float currentIncreaseWeatherIntensity;
    private float currentDecreaseWeatherIntensity;
    public void SetWeather(float weatherType)
    {
        var previousWeather = WeatherOptions.FirstOrDefault(c => c.Type == currentWeather);
        currentWeather = (WeatherType)weatherType;
        if (previousWeather != null)
        {
            if (previousWeather.Type != WeatherType.None)
            {
                previousWeather.Action = WeatherAction.Decrease;
                currentDecreaseWeatherIntensity = previousWeather.MaxIntensity;
            }
            else
                previousWeather.Action = WeatherAction.None;
        }
        var activeWeather = WeatherOptions.First(c => c.Type == currentWeather);
        if (activeWeather.Type != WeatherType.None)
        {
            activeWeather.Particles.gameObject.SetActive(true);
            activeWeather.Action = WeatherAction.Increase;
            var emission = activeWeather.Particles.emission;
            emission.rateOverTime = 0;
            currentIncreaseWeatherIntensity = 0;
        }
        else
            activeWeather.Action = WeatherAction.None;
    }

    private void Update()
    {
        var newWeather = WeatherOptions.FirstOrDefault(c => c.Action == WeatherAction.Increase);
        if (newWeather != null)
        {
            if (newWeather.MaxIntensity > newWeather.Particles.emission.rateOverTime.constant)
            {
                var emission = newWeather.Particles.emission;
                currentIncreaseWeatherIntensity += Time.deltaTime * newWeather.IncreaseIntensitySpeed;
                emission.rateOverTime = currentIncreaseWeatherIntensity;
            }
            else
                newWeather.Action = WeatherAction.None;
        }
        var oldWeather = WeatherOptions.FirstOrDefault(c => c.Action == WeatherAction.Decrease);
        if (oldWeather != null)
        {
            if (oldWeather.Particles.emission.rateOverTime.constant > 0)
            {
                var emission = oldWeather.Particles.emission;
                currentDecreaseWeatherIntensity -= Time.deltaTime * oldWeather.DecreaseIntensitySpeed;
                emission.rateOverTime = currentDecreaseWeatherIntensity;
            }
            else
            {
                oldWeather.Action = WeatherAction.None;
                StartCoroutine(DisableWeatherCo(oldWeather));
            }
        }

        transform.position = FollowTarget.transform.position;
    }

    private IEnumerator DisableWeatherCo(Weather w)
    {
        yield return new WaitForSeconds(5f);
        w.Particles.gameObject.SetActive(false);
    }
}
