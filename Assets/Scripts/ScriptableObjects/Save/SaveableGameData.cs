using System.Collections.Generic;

[System.Serializable]
public class SaveableGameData
{
    public SaveableGameData()
    {
        ScenesData = new List<SaveableSceneData>();
    }

    public List<SaveableSceneData> ScenesData;
    public SaveablePlayerData PlayerData;
}

[System.Serializable]
public class SaveablePlayerData
{
    public SaveablePlayerData()
    {
        UnlockedSpellIds = new List<string>();
        BoundSpellIds = new List<string>();
    }

    public float PositionX;
    public float PositionY;
    public List<string> UnlockedSpellIds;
    public List<string> BoundSpellIds;
}

[System.Serializable]
public class SaveableSceneData
{
    public SaveableSceneData()
    {
        Enemies = new List<SaveableEnemyData>();
        Interactables = new List<SaveableInteractableData>();
        Triggers = new List<SaveableTriggerData>();
    }

    public string SceneName;
    public bool IsCurrentScene;
    public List<SaveableEnemyData> Enemies;
    public List<SaveableInteractableData> Interactables;
    public List<SaveableTriggerData> Triggers;
}

[System.Serializable]
public class SaveableEnemyData
{
    public string EnemyId;
    public bool IsAlive;
    public float BodyPositionX;
    public float BodyPositionY;
}

[System.Serializable]
public class SaveableInteractableData
{
    public string InteractableId;
    public bool CanBeInteractedWith;
}

[System.Serializable]
public class SaveableTriggerData
{
    public string TriggerId;
    public bool IsActive;
}

[System.Serializable]
public class SaveableSaveFileData
{
    public string FileName;
    public string CurrentSceneName;
    public string LastPlayed;
    public List<string> SpellsIds;
}
