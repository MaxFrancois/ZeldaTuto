using System.Collections;
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
        SpellCategories.Select(c => c.Spells.First(d => d == spell)).First().IsUnlocked = true;
    }

    public void LockSpell(SpellConfig spell)
    {
        SpellCategories.Select(c => c.Spells.First(d => d == spell)).First().IsUnlocked = false;
    }

    public List<SpellConfig> GetAllSpells()
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
}
