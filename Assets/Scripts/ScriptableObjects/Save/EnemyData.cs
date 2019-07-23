using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "SaveData/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Static values")]
    public string EnemyId;
    public bool IsAliveDefaultValue;
    public bool LeaveBody;

    [Header("Variable values")]
    public bool IsAlive;
    public Vector2 BodyPosition;

    public SaveableEnemyData GetEnemySaveData()
    {
        return new SaveableEnemyData() {
            EnemyId = EnemyId,
            IsAlive = IsAlive,
            BodyPositionX = BodyPosition.x,
            BodyPositionY = BodyPosition.y,
        };
    }

    public void LoadEnemyData(SaveableEnemyData data)
    {
        IsAlive = data.IsAlive;
        BodyPosition = new Vector2(data.BodyPositionX, data.BodyPositionY);
    }
}
