using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "SaveData/GameData")]
public class GameData : ScriptableObject
{
    public List<SceneData> SceneData;
    public PlayerData PlayerData;
}
