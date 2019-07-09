using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneData", menuName = "SaveData/SceneData")]
[Serializable]
public class SceneData: ScriptableObject
{
    public string SceneName;
    public bool IsCurrentScene;
    public List<Enemy> Enemies;
    public List<ScriptableObject> Interactables;
}