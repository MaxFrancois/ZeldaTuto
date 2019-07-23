using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneData", menuName = "SaveData/SceneData")]
public class SceneData: ScriptableObject
{
    public string SceneName;
    public bool IsCurrentScene;
    public List<EnemyData> Enemies;
    public List<InteractableData> Interactables;
    public List<TriggerData> Triggers;
    //TODO: public List<DestructibleData> Destructibles;

    public SaveableSceneData GetSaveSceneData()
    {
        var saveableSceneData = new SaveableSceneData();
        saveableSceneData.IsCurrentScene = SceneName == SceneManager.GetActiveScene().name;
        saveableSceneData.SceneName = SceneName;
        foreach (var enemy in Enemies)
            saveableSceneData.Enemies.Add(enemy.GetEnemySaveData());
        foreach (var interactable in Interactables)
            saveableSceneData.Interactables.Add(interactable.GetInteractableSaveData());
        foreach (var trigger in Triggers)
            saveableSceneData.Triggers.Add(trigger.GetTriggerSaveData());
        return saveableSceneData;
    }

    public void LoadSceneData(SaveableSceneData data)
    {
        foreach (var enemyData in data.Enemies)
        {
            var enemy = Enemies.FirstOrDefault(c => c.EnemyId == enemyData.EnemyId);
            if (enemy) enemy.LoadEnemyData(enemyData);
            else Debug.LogError("Couldn't load enemy with id " + enemyData.EnemyId);
        }

        foreach (var interactableData in data.Interactables)
        {
            var interactable = Interactables.FirstOrDefault(c => c.InteractableId == interactableData.InteractableId);
            if (interactable) interactable.LoadInteractableData(interactableData);
            else Debug.LogError("Couldn't load interactable with id " + interactableData.InteractableId);
        }

        foreach (var triggerData in data.Triggers)
        {
            var trigger = Triggers.FirstOrDefault(c => c.TriggerId == triggerData.TriggerId);
            if (trigger) trigger.LoadTriggerData(triggerData);
            else Debug.LogError("Couldn't load trigger with id " + triggerData.TriggerId);
        }
    }
}