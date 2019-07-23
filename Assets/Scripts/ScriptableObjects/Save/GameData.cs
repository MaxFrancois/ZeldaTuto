using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "GameData", menuName = "SaveData/GameData")]

public class GameData : ScriptableObject
{
    public List<SceneData> ScenesData;
    public PlayerData PlayerData;
    public string CurrentFileName;
    public string LastSavedFileName;

    public void SetNewFileName()
    {
        var path = Application.persistentDataPath;
        var nbOfSaves = Directory.GetFiles(path).Where(c => c.Contains(".dat")).Count();
        CurrentFileName = "GameData" + nbOfSaves + ".dat";
    }

    public SaveableGameData ConvertToSaveableGameData()
    {
        var saveableGameData = new SaveableGameData();
        saveableGameData.PlayerData = PlayerData.GetSavePlayerData();
        foreach (var scene in ScenesData)
            saveableGameData.ScenesData.Add(scene.GetSaveSceneData());
        return saveableGameData;
    }

    public void LoadFromSaveableGameData(SaveableGameData data)
    {
        PlayerData.LoadPlayerData(data.PlayerData);
        foreach (var scene in ScenesData)
        {
            var s = data.ScenesData.First(c => c.SceneName == scene.SceneName);
            scene.LoadSceneData(s);
        }
    }

    public List<SaveFileData> GetSaveFileData(List<SaveableSaveFileData> data)
    {
        var ret = new List<SaveFileData>();
        foreach (var file in data)
        {
            var f = new SaveFileData()
            {
                FileName = file.FileName,
                CurrentSceneName = file.CurrentSceneName,
                LastPlayed = file.LastPlayed,
            };
            f.Spells = new List<SpellConfig>();
            foreach (var spellId in file.SpellsIds)
                f.Spells.Add(PlayerData.SpellBook.GetSpellById(spellId));
            ret.Add(f);
        }
        return ret;
    }

    public List<SaveableSaveFileData> ToSaveableSaveFileData(List<SaveFileData> data)
    {
        var ret = new List<SaveableSaveFileData>();
        foreach (var d in data)
            ret.Add(ToSaveableSaveFileData(d));
        return ret;
    }

    public SaveableSaveFileData ToSaveableSaveFileData(SaveFileData data)
    {
        var ret = new SaveableSaveFileData();
        ret.FileName = data.FileName;
        ret.CurrentSceneName = data.CurrentSceneName;
        ret.LastPlayed = data.LastPlayed;
        ret.SpellsIds = new List<string>();
        foreach (var spell in data.Spells)
            ret.SpellsIds.Add(spell.Id);
        return ret;
    }
}
