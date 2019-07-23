using UnityEngine;

[CreateAssetMenu(fileName = "InteractableData", menuName = "SaveData/InteractableData")]
public class InteractableData : ScriptableObject
{
    [Header("Static values")]
    public string InteractableId;
    public bool CanBeInteractedWithDefaultValue;
    public bool HideWhenCannotInteract;
    public Sprite SpriteAfterInteraction;

    [Header("Variable values")]
    public bool CanBeInteractedWith;


    public SaveableInteractableData GetInteractableSaveData()
    {
        return new SaveableInteractableData()
        {
            InteractableId = InteractableId,
            CanBeInteractedWith = CanBeInteractedWith
        };
    }

    public void LoadInteractableData(SaveableInteractableData data)
    {
        CanBeInteractedWith = data.CanBeInteractedWith;
    }
}
