using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestMenu : MonoBehaviour
{
    public List<Button> Buttons;
    private int selectedButtonIndex = 0;
    public GameObject SpellBookMenu;
    private float recentlyMoved;
    private float recentlyPressed;
    [SerializeField] GameObject FadePanel;
    [SerializeField] float FadeDuration;

    private void OnEnable()
    {
        recentlyMoved = 0f;
        recentlyPressed = 0f;
        Buttons[selectedButtonIndex].Select();
    }

    private void Update()
    {
        if (recentlyMoved > 0)
            recentlyMoved -= Time.deltaTime;
        if (recentlyPressed > 0)
            recentlyPressed -= Time.deltaTime;

        if (recentlyMoved <= 0)
        {
            var input = Input.GetAxisRaw("Vertical");
            if (input != 0)
            {
                recentlyMoved = 1f;
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
        if (recentlyPressed <= 0)
        {
            if (Input.GetButtonDown("Spell 0"))
            {
                recentlyPressed = 1f;
                Buttons[selectedButtonIndex].onClick.Invoke();
            }
            if (Input.GetButtonDown("Spell 1"))
            {
                recentlyPressed = 1f;
                Resume();
            }
        }
    }

    public void SpellBook()
    {
        SpellBookMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Teleport()
    {
        Debug.Log("Teleport");
    }

    public void SaveExit()
    {
        StartCoroutine(SaveExitCo());
    }

    IEnumerator SaveExitCo()
    {
        SaveManager.instance.SaveGame();
        Instantiate(FadePanel, Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(FadeDuration);
        SceneManager.LoadScene("MainMenu");
    }

    public void Resume()
    {
        selectedButtonIndex = 0;
        MenuManager.StartTime();
        this.gameObject.SetActive(false);
    }
}
