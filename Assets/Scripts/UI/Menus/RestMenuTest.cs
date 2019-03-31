using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RestMenuTest : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject selectedObject;
    public GameObject SpellBookMenu;
    private bool buttonSelected;

    private void OnEnable()
    {
        eventSystem.SetSelectedGameObject(selectedObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Vertical") != 0 && !buttonSelected)
        {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }
        if (Input.GetButtonDown("Spell 3"))
        {
            Resume();
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
        Debug.Log("Save & Exit");
    }

    public void Resume()
    {
        MenuManager.StartTime();
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        eventSystem.SetSelectedGameObject(null);
        buttonSelected = false;
    }
}
