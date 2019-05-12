using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpellBook : ScriptableObject
{
    public List<SpellConfig> Spells;

    public void AddSpell(SpellConfig spell)
    {
        if (!Spells.Contains(spell))
            Spells.Add(spell);
    }

    public void RemoveSpell(SpellConfig spell)
    {
        if (Spells.Contains(spell))
            Spells.Remove(spell);
    }
}
