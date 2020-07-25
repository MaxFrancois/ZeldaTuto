using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RestMenu : MonoBehaviour
{
    [SerializeField] GameObject firstButton = default;
    [SerializeField] GameObject spellBookMenu = default;
    [SerializeField] FadePanelSignal fadeToSignal = default;
    bool isFading;

    EventSystem _eventSystem;
    EventSystem EventSystem
    {
        get
        {
            if (_eventSystem == null)
                _eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
            return _eventSystem;
        }
    }

    void OnEnable()
    {
        isFading = false;
        StartCoroutine(SelectDefaultButton());
    }

    IEnumerator SelectDefaultButton()
    {
        yield return new WaitForSeconds(0.1f);
        EventSystem.SetSelectedGameObject(firstButton);
    }

    void OnDisable()
    {
        EventSystem.SetSelectedGameObject(null);
    }

    void Update()
    {
        if (Input.GetButtonDown("Spell 3") && !isFading)
            Resume();
    }

    public void Resume()
    {
        MenuManager.StartTime();
        gameObject.SetActive(false);
    }

    public void SpellBook()
    {
        spellBookMenu.SetActive(true);
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
        isFading = true;
        PermanentObjects.Instance.SaveManager.SaveGame();
        var loading = SceneManager.LoadSceneAsync("MainMenu");
        loading.allowSceneActivation = false;
        fadeToSignal.Raise();
        yield return new WaitForSeconds(fadeToSignal.Duration);
        loading.allowSceneActivation = true;
    }
}
