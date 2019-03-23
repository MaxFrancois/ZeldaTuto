using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public List<Button> Buttons;
    private int selectedButtonIndex;
    private bool recentlyPressed = false;

    private void OnEnable()
    {
        selectedButtonIndex = 0;
        recentlyPressed = false;
        Buttons[selectedButtonIndex].Select();
    }

    void Update()
    {
        if (Input.GetButtonDown("Start") || Input.GetButtonDown("Spell 3"))
        {
            Resume();
        }
        else
        {
            var input = Input.GetAxisRaw("Vertical");
            if (input != 0)
            {
                if (!recentlyPressed)
                {
                    recentlyPressed = true;
                    if (input < 0)
                    {
                        selectedButtonIndex++;
                        if (selectedButtonIndex > Buttons.Count - 1)
                            selectedButtonIndex = 0;

                    }
                    else
                    {
                        selectedButtonIndex--;
                        if (selectedButtonIndex < 0)
                            selectedButtonIndex = Buttons.Count - 1;
                    }
                    Buttons[selectedButtonIndex].Select();
                }
            }
            else
                recentlyPressed = false;
            if (Input.GetButtonDown("Spell 0"))
            {
                Buttons[selectedButtonIndex].onClick.Invoke();
            }
        }
    }

    public void Resume()
    {
        MenuManager.StartTime();
        gameObject.SetActive(false);
    }

    public void LoadSettings()
    {

    }

    public void LoadMainMenu()
    {
        MenuManager.StartTime();
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
