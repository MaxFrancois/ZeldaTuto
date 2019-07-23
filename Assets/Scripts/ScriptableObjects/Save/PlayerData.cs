using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "SaveData/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Runtime Values")]
    public SpellBook SpellBook;
    public Vector2 PlayerPosition;
    public List<SpellConfig> Spells;

    [Header("New Game Start values")]
    public Vector2 StartPosition;
    public List<SpellConfig> StartSpells;

    PlayerMovement _player;
    PlayerMovement Player
    {
        get
        {
            if (_player == null)
            {
                var p = GameObject.FindWithTag("Player");
                if (p != null) _player = p.GetComponent<PlayerMovement>();
                else Debug.LogError("COULDNT FIND PLAYER IN PLAYERDATA");
            }
            return _player;
        }
    }

    public void Reset()
    {
        SpellBook.LockAll();
        foreach (var spell in StartSpells)
            SpellBook.UnlockSpell(spell);
        PlayerPosition = StartPosition;
        Spells = StartSpells;
    }

    public SaveablePlayerData GetSavePlayerData()
    {
        var saveablePlayerData = new SaveablePlayerData();
        saveablePlayerData.PositionX = Player.transform.position.x;
        saveablePlayerData.PositionY = Player.transform.position.y;
        Spells = Player.GetComponent<SpellBar>().Spells;
        foreach (var spell in Spells)
            if (spell)
                saveablePlayerData.BoundSpellIds.Add(spell.Id);
        foreach (var spellCategory in SpellBook.SpellCategories)
            foreach (var spell in spellCategory.Spells)
                if (spell.IsUnlocked)
                    saveablePlayerData.UnlockedSpellIds.Add(spell.Id);

        return saveablePlayerData;
    }

    public void LoadPlayerData(SaveablePlayerData data)
    {
        PlayerPosition = new Vector2(data.PositionX, data.PositionY);
        foreach (var spellCategory in SpellBook.SpellCategories)
            foreach (var spell in spellCategory.Spells)
                spell.IsUnlocked = data.UnlockedSpellIds.Contains(spell.Id);
        Spells = new List<SpellConfig>();
        foreach (var boundSpell in data.BoundSpellIds)
            Spells.Add(SpellBook.GetSpellById(boundSpell));
    }
}
