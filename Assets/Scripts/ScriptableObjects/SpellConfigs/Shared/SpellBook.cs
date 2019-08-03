using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class SpellCategory
{
    public SpellElement Element;
    public List<SpellConfig> Spells;
}

[CreateAssetMenu]
public class SpellBook : ScriptableObject
{
    public List<SpellCategory> SpellCategories;

    public void AddSpell(SpellConfig spell)
    {
        var category = SpellCategories.First(c => c.Element == spell.Element);
        if (!category.Spells.Contains(spell))
            category.Spells.Add(spell);
    }

    public void RemoveSpell(SpellConfig spell)
    {
        var category = SpellCategories.First(c => c.Element == spell.Element);
        if (category.Spells.Contains(spell))
            category.Spells.Remove(spell);
    }

    public void UnlockSpell(SpellConfig spell)
    {
        if (spell != null)
        {
            foreach (var category in SpellCategories)
            {
                var cfg = category.Spells.FirstOrDefault(d => d == spell);
                if (cfg) { cfg.IsUnlocked = true; break; }
            }
        }
    }

    public void LockAll()
    {
        foreach (var spellCategory in SpellCategories)
            foreach (var spell in spellCategory.Spells)
                LockSpell(spell);
    }

    public void LockSpell(SpellConfig spell)
    {
        foreach (var category in SpellCategories)
        {
            var cfg = category.Spells.FirstOrDefault(d => d == spell);
            if (cfg) { cfg.IsUnlocked = false; break; }
        }
    }

    public List<SpellConfig> GetAllUnlockedSpells()
    {
        var spells = new List<SpellConfig>();
        foreach (var category in SpellCategories)
            foreach (var spell in category.Spells)
                if (spell.IsUnlocked)
                    spells.Add(spell);
        return spells;
    }

    public List<SpellConfig> GetByElement(SpellElement element)
    {
        return SpellCategories.FirstOrDefault(c => c.Element == element)?.Spells;
    }

    public List<SpellConfig> GetUnlockedSpellsByElement(SpellElement element)
    {
        return SpellCategories.FirstOrDefault(c => c.Element == element)?.Spells.Where(d => d.IsUnlocked).ToList();
    }

    public SpellConfig GetSpellById(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;
        foreach (var category in SpellCategories)
        {
            var spell = category.Spells.FirstOrDefault(d => d.Id == id);
            if (spell) return spell;
        }
        Debug.LogError("Couldnt get spell with ID " + id);
        return null;
    }
}
