using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayTimeManager : MonoBehaviour
{
    public static DayTimeManager instance;

    private bool isGettingBrighter, isGettingDarker, isDayTime, isNightTime;
    public float TransitionSpeed;
    //public float dayAndNightDuration = 45f;  // 900 = 15 minutes
    public float DayTimeDuration;
    public float NightTimeDuration;
    private float timeTracker;
    public float MaxLevelDarkness = 0.75f;
    public float LightThreshold;
    private SpriteRenderer darknessImage;
    public BoolSignal PlayerLightSignal;
    private bool isLightOn = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        darknessImage = GetComponent<SpriteRenderer>();
        InitializeDayAndNightCycle();
    }

    void Update()
    {
        DayNightCycle();
        transform.position = PermanentObjects.Instance.Player.transform.position;
    }

    void InitializeDayAndNightCycle()
    {
        isDayTime = true;
        isNightTime = false;
        isGettingBrighter = false;
        isGettingDarker = false;
        timeTracker = DayTimeDuration;
    }

    void DayNightCycle()
    {
        timeTracker -= Time.deltaTime;

        if (timeTracker <= 0)
        {
            if (isDayTime)
            {
                isDayTime = false;
                isGettingDarker = true;
            }
            else if (isNightTime)
            {
                isNightTime = false;
                isGettingBrighter = true;
            }
        }

        if (isGettingBrighter)
        {
            darknessImage.color = new Color(darknessImage.color.r, darknessImage.color.g, darknessImage.color.b,
                Mathf.MoveTowards(darknessImage.color.a, 0f, TransitionSpeed * Time.deltaTime));

            if (darknessImage.color.a <= LightThreshold && isLightOn)
            {
                PlayerLightSignal.Raise(false);
                isLightOn = false;
            }

            if (darknessImage.color.a == 0f)
            {
                isGettingBrighter = false;
                isDayTime = true;
                timeTracker = DayTimeDuration;
            }
        }

        if (isGettingDarker)
        {
            darknessImage.color = new Color(darknessImage.color.r, darknessImage.color.g, darknessImage.color.b,
                Mathf.MoveTowards(darknessImage.color.a, MaxLevelDarkness, TransitionSpeed * Time.deltaTime));
            if (darknessImage.color.a >= LightThreshold && !isLightOn)
            {
                PlayerLightSignal.Raise(true);
                isLightOn = true;
            }
            if (darknessImage.color.a >= MaxLevelDarkness)
            {
                isGettingDarker = false;
                isNightTime = true;
                timeTracker = NightTimeDuration;
            }
        }
    }
}
