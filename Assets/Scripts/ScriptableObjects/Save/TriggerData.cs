using UnityEngine;

[CreateAssetMenu(fileName = "TriggerData", menuName = "SaveData/TriggerData")]
public class TriggerData : ScriptableObject
{
    [Header("Static values")]
    public string TriggerId;
    public bool IsActiveDefaultValue;

    [Header("Variable values")]
    public bool IsActive;

    public SaveableTriggerData GetTriggerSaveData()
    {
        return new SaveableTriggerData()
        {
            TriggerId = TriggerId,
            IsActive = IsActive
        };
    }

    public void LoadTriggerData(SaveableTriggerData data)
    {
        IsActive = data.IsActive;
    }
}
