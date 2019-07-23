using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    [SerializeField] GameData GameData;
    string gamePath;
    const string saveFilesFile = "gameSummaryFile.menu";
    List<EnemyBase> currentSceneEnemies;
    List<Interactable> currentSceneInteractables;
    List<Trigger> currentSceneTriggers;
    List<SaveFileData> saveFiles = new List<SaveFileData>();

    void Awake()
    {
        gamePath = Application.persistentDataPath;
        var summaryFilePath = Path.Combine(gamePath, saveFilesFile);
        if (File.Exists(summaryFilePath))
        {
            using (Stream s = File.Open(summaryFilePath, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                var saveableGameData = (List<SaveableSaveFileData>)bf.Deserialize(s);
                saveFiles = GameData.GetSaveFileData(saveableGameData);
            }
        }
        currentSceneEnemies = new List<EnemyBase>();
        currentSceneInteractables = new List<Interactable>();
        currentSceneTriggers = new List<Trigger>();
    }

    public List<SaveFileData> GetSaveFiles()
    {
        return saveFiles;
    }

    public void StartGame(StartMenuAction action, string fileName)
    {
        if (action == StartMenuAction.NewGame)
            NewGame();
        else if (action == StartMenuAction.Continue)
            LoadGame(GameData.LastSavedFileName);
        else if (action == StartMenuAction.Load)
            LoadGame(fileName);
    }

    public void SaveGame()
    {
        try
        {
            if (!Directory.Exists(gamePath))
                Directory.CreateDirectory(gamePath);
            var saveableGameData = GameData.ConvertToSaveableGameData();
            using (Stream s = File.Open(Path.Combine(gamePath, GameData.CurrentFileName), FileMode.OpenOrCreate))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(s, saveableGameData);
            }

            //update saveFliesTracker
            var summaryFilePath = Path.Combine(gamePath, saveFilesFile);
            SaveFileData saveFileData = saveFiles.FirstOrDefault(c => c.FileName == GameData.CurrentFileName);
            if (saveFileData == null)
            {
                saveFileData = new SaveFileData() { FileName = GameData.CurrentFileName };
                saveFiles.Add(saveFileData);
            }
            saveFileData.LastPlayed = DateTime.Now.ToShortDateString();
            saveFileData.CurrentSceneName = SceneManager.GetActiveScene().name;
            saveFileData.Spells = GameData.PlayerData.Spells;
            var saveableSaveFileData = GameData.ToSaveableSaveFileData(saveFiles);
            using (Stream s = File.Open(Path.Combine(gamePath, summaryFilePath), FileMode.OpenOrCreate))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(s, saveableSaveFileData);
            }

            GameData.LastSavedFileName = GameData.CurrentFileName;
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to save game: " + ex.Message);
        }
    }

    public void LoadGame(string fileName = "")
    {
        try
        {
            if (string.IsNullOrEmpty(fileName)) fileName = GameData.LastSavedFileName;
            using (Stream s = File.Open(Path.Combine(gamePath, fileName), FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                var saveableGameData = (SaveableGameData)bf.Deserialize(s);
                GameData.LoadFromSaveableGameData(saveableGameData);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to Load game: " + ex.Message);
        }
        GameData.CurrentFileName = fileName;
        var sceneName = saveFiles.First(c => c.FileName == fileName).CurrentSceneName;
        StartCoroutine(LoadScene(sceneName));
    }

    public void NewGame()
    {
        ResetAll(true);
        StartCoroutine(LoadScene("dungeon"));
        //GameData.PlayerData.Reset();
    }

    public void ResetAll(bool hardReset)
    {
        foreach (var scene in GameData.ScenesData)
            ResetScene(scene, hardReset);
        if (hardReset)
        {
            GameData.SetNewFileName();
            GameData.PlayerData.Reset();
        }
    }

    public void ResetScene(SceneData scene, bool hardReset)
    {
        bool isCurrentScene = false;
        if (scene == null)
        {
            scene = GameData.ScenesData.First(c => c.IsCurrentScene);
            isCurrentScene = true;
        }
        else isCurrentScene = scene.SceneName == SceneManager.GetActiveScene().name;
        ResetEnemies(scene, isCurrentScene);
        if (hardReset)
        {
            ResetInteractables(scene, isCurrentScene);
            ResetTriggers(scene, isCurrentScene);
        }
        StartCoroutine(LoadScene());
    }

    public void ResetTriggers(SceneData scene, bool isCurrentScene)
    {
        foreach (var trigger in scene.Triggers)
        {
            trigger.IsActive = trigger.IsActiveDefaultValue;
            //if (isCurrentScene)
            //    currentSceneTriggers.First(c => c.ID == trigger.TriggerId).gameObject.SetActive(trigger.IsActive);
        }
    }

    public void ResetInteractables(SceneData scene, bool isCurrentScene)
    {
        foreach (var interactable in scene.Interactables)
        {
            interactable.CanBeInteractedWith = interactable.CanBeInteractedWithDefaultValue;
            //if (isCurrentScene)
            //    currentSceneInteractables.First(c => c.ID == interactable.InteractableId).Reset();
        }
    }

    public void ResetEnemies(SceneData scene, bool isCurrentScene)
    {
        foreach (var enemy in scene.Enemies)
        {
            enemy.IsAlive = enemy.IsAliveDefaultValue;
            //if (isCurrentScene)
            //{
            //    var e = currentSceneEnemies.FirstOrDefault(c => c.ID == enemy.EnemyId);
            //    if (enemy.IsAlive)
            //    {
            //        e.gameObject.SetActive(true);
            //        e.Reset();
            //    }
            //    else e.gameObject.SetActive(false);
            //}
        }
    }

    IEnumerator LoadScene(string sceneName = "")
    {
        if (string.IsNullOrEmpty(sceneName))
            sceneName = GameData.ScenesData.First(c => c.IsCurrentScene).SceneName;
        var asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        PermanentObjects.Instance.EnableVisible();
        // if OnLevelWasLoaded doesnt work to get saveable objects, trythis
        //var currentScene = SceneManager.GetSceneByName(sceneName);
        //currentScene.
    }

    private void OnLevelWasLoaded(int level)
    {
        //var v = FindObjectsOfType<EnemyBase>();
        //var enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        //currentSceneEnemies.Clear();
        //foreach (var t in enemies)
        //{
        //    var script = t.GetComponent<EnemyBase>();
        //    if (script) currentSceneEnemies.Add(script);
        //}

        //var allenemies = Resources.FindObjectsOfTypeAll<EnemyBase>().OrderBy(c => c.name);
        var rooms = FindObjectsOfType<Room>();
        currentSceneEnemies = new List<EnemyBase>();
        currentSceneInteractables = new List<Interactable>();
        currentSceneTriggers = new List<Trigger>();

        foreach (var room in rooms)
        {
            currentSceneEnemies.AddRange(room.GetComponentsInChildren<EnemyBase>(true));
            currentSceneInteractables.AddRange(room.GetComponentsInChildren<Interactable>(true));
            currentSceneTriggers.AddRange(room.GetComponentsInChildren<Trigger>(true));
        }

        //FindObjectsOfType<EnemyBase>());
        //currentSceneInteractables.AddRange(FindObjectsOfType<Interactable>());
        //currentSceneTriggers.AddRange(FindObjectsOfType<Trigger>());
    }
}