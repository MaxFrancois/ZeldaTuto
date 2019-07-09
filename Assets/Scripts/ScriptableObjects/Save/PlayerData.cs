using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "SaveData/PlayerData")]
[Serializable]
public class PlayerData : ScriptableObject
{
    public float PositionX;
    public float PositionY;
    public SpellBook SpellBook;
    public List<SpellConfig> Spells;

    private GameObject player;
    private SpellBar spellBar;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        spellBar = player.GetComponent<SpellBar>();
    }

    public void SavePlayerData()
    {
        Spells = spellBar.Spells;
        PositionX = player.transform.position.x;
        PositionX = player.transform.position.y;
    }

    public void LoadPlayerData()
    {
        spellBar.Spells = Spells;
        player.transform.position = new Vector3(PositionX, PositionY, 0);
    }
}
