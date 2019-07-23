using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public enum StartMenuAction
{
    NewGame = 0,
    Continue = 1,
    Load = 2
}

public class MainMenu : MonoBehaviour
{
    [SerializeField] EventSystem eventSystem;

    [Header("Fade")]
    [SerializeField] GameObject fadeToPanel;
    [SerializeField] float fadeDuration;
    [SerializeField] GameObject canvas;

    [Header("Orbs")]
    [SerializeField] GameObject orbsRotationCenter;
    [SerializeField] List<GameObject> orbs;
    [SerializeField] float rotationSpeed;
    [SerializeField] float orbsOffsetY;

    [Header("MainMenu")]
    [SerializeField] GameObject mainMenuParent;
    [SerializeField] GameObject SmallMenu;
    [SerializeField] Button SmallMenuStartbutton;
    [SerializeField] GameObject LongMenu;
    [SerializeField] Button LongMenuStartbutton;

    [Header("SaveFilesMenu")]
    [SerializeField] GameObject saveFilesMenuParent;
    [SerializeField] GameObject saveFilePrefab;
    [SerializeField] float saveFilesOffsetY;
    List<SaveFileData> saveFiles;
    List<GameObject> saveFileObjects;

    [Header("SettingsMenu")]
    [SerializeField] GameObject settingsMenuParent;

    bool canGoBack;

    void Start()
    {
        saveFiles = PermanentObjects.Instance.SaveManager.GetSaveFiles();
        PermanentObjects.Instance.DisableVisible();
        LoadMainMenu();

        saveFileObjects = new List<GameObject>();
        for (int i = 0; i < saveFiles.Count; i++)
        {
            var saveFileCpn = Instantiate(saveFilePrefab, saveFilesMenuParent.transform.position, Quaternion.identity, saveFilesMenuParent.transform);
            saveFileCpn.transform.position = new Vector3(saveFileCpn.transform.position.x, saveFileCpn.transform.position.y + (saveFilesOffsetY * i), 0);
            saveFileCpn.GetComponent<MainMenuSaveFileButton>().Initialize(saveFiles[i]);
            var fileName = saveFiles[i].FileName;
            saveFileCpn.GetComponent<MainMenuButton>().onClick.AddListener(delegate { LoadFile(fileName); });
            saveFileObjects.Add(saveFileCpn);
        }

        for (int i = 0; i < orbs.Count; i++)
        {
            Vector3 spawnPosition = new Vector3(orbsRotationCenter.transform.position.x, orbsRotationCenter.transform.position.y + orbsOffsetY, 0);
            var instance = Instantiate(orbs[i], spawnPosition, Quaternion.identity, orbsRotationCenter.transform);
            instance.transform.rotation = new Quaternion(0, 0, 0, 0);
            orbsRotationCenter.transform.Rotate(Vector3.back, 360 / orbs.Count);
        }
    }

    void LoadMainMenu()
    {
        canGoBack = false;
        mainMenuParent.SetActive(true);
        saveFilesMenuParent.SetActive(false);
        settingsMenuParent.SetActive(false);
        if (saveFiles.Count == 0)
        {
            SmallMenu.SetActive(true);
            LongMenu.SetActive(false);
            eventSystem.SetSelectedGameObject(SmallMenuStartbutton.gameObject);
        }
        else
        {
            SmallMenu.SetActive(false);
            LongMenu.SetActive(true);
            eventSystem.SetSelectedGameObject(LongMenuStartbutton.gameObject);
        }
    }

    void Update()
    {
        orbsRotationCenter.transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
        if (orbsRotationCenter.transform.rotation.z <= -360)
            orbsRotationCenter.transform.rotation = new Quaternion(50, 0, 0, 0);
        if (canGoBack && Input.GetButtonDown("Spell 3"))
            LoadMainMenu();
    }

    public void NewGame()
    {
        StartCoroutine(StartGame(StartMenuAction.NewGame));
    }

    public void Continue()
    {
        StartCoroutine(StartGame(StartMenuAction.Continue));
    }

    public void LoadSaveFilesMenu()
    {
        mainMenuParent.SetActive(false);
        saveFilesMenuParent.SetActive(true);
        canGoBack = true;
        eventSystem.SetSelectedGameObject(saveFileObjects[0]);
    }

    //public void LoadFile(Button btn)
    //{
    //    var saveFile = btn.GetComponent<MainMenuSaveFileButton>();
    //    StartCoroutine(StartGame(StartMenuAction.Load, saveFile.GetSaveFileName()));
    //}

    public void LoadFile(string fileName)
    {
        StartCoroutine(StartGame(StartMenuAction.Load, fileName));
    }

    public void LoadSettingsMenu()
    {
        mainMenuParent.SetActive(false);
        settingsMenuParent.SetActive(true);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    IEnumerator StartGame(StartMenuAction action, string fileName = "")
    {
        if (fadeToPanel != null)
        {
            Instantiate(fadeToPanel, Vector3.zero, Quaternion.identity, canvas.transform);
        }
        yield return new WaitForSeconds(fadeDuration);

        PermanentObjects.Instance.SaveManager.StartGame(action, fileName);
        //PermanentObjects.Instance.EnableVisible();
        //PermanentObjects.Instance.SaveManager.ResetAll(true);
        //var asyncOperation = SceneManager.LoadSceneAsync("Dungeon");
        //while (!asyncOperation.isDone)
        //{
        //    yield return null;
        //}
    }
}
