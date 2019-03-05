using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpellBook : ScriptableObject
{
    public List<Spell> Spells;

    public void AddSpell(Spell spell)
    {
        if (!Spells.Contains(spell))
            Spells.Add(spell);
    }

    public void RemoveSpell(Spell spell)
    {
        if (Spells.Contains(spell))
            Spells.Remove(spell);
    }
}
