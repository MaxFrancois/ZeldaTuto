using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject RestMenu;
    //public GameObject SpellBookMenu;
    //public GameObject TalentMenu;
    public static bool IsPaused = false;
    private static float startTimeCountdown = 0f;
    public static bool RecentlyUnpaused = false;

    void Update()
    {
        if (startTimeCountdown > 0)
            startTimeCountdown -= Time.deltaTime;
        if (startTimeCountdown <= 0 && RecentlyUnpaused)
            RecentlyUnpaused = false;
        if (Input.GetButtonDown("Start"))
        {
            if (!IsPaused)
                OpenPauseMenu();
        }
    }

    public static void StopTime()
    {
        IsPaused = true;
        Time.timeScale = 0f;
    }

    public static void StartTime()
    {
        RecentlyUnpaused = true;
        startTimeCountdown = 0.2f;
        IsPaused = false;
        Time.timeScale = 1f;
    }

    public void OpenPauseMenu()
    {
        StopTime();
        PauseMenu.SetActive(true);
    }

    public void OpenRestMenu()
    {
        if (!RecentlyUnpaused)
        {
            StopTime();
            RestMenu.SetActive(true);
        }
    }

    //public void OpenSpellBookMenu()
    //{
    //    StopTime();
    //    SpellBookMenu.SetActive(true);
    //}

    //public void OpenTalentMenu()
    //{
    //    StopTime();
    //    TalentMenu.SetActive(true);
    //}
}
