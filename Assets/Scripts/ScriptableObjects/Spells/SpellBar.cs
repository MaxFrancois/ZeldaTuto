using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpellBar : ScriptableObject
{
    public List<Spell> Spells;
    public int MaxQuantity;
    private Spell currentSpell;

    public void AddSpell(Spell spell)
    {
        if (!Spells.Contains(spell) && Spells.Count < MaxQuantity)
            Spells.Add(spell);
    }

    public void RemoveSpell(Spell spell)
    {
        if (Spells.Contains(spell))
            Spells.Remove(spell);
    }

    public void CastSpell(int spellIndex, Transform source, Vector3 direction)
    {
        if (spellIndex < MaxQuantity && spellIndex >= 0)
            if (Spells[spellIndex] != null)
            {
                var spell = Instantiate(Spells[spellIndex], source.position, Quaternion.identity);
                spell.Cast(source, direction);
                //try
                //{
                //    Destroy(spell.AnimationInstance, spell.LifeTime);
                //}
                //catch (Exception e) { Debug.Log("failed to delete AnimationInstance" + e.Message); }
                //Destroy(spell.gameObject, spell.LifeTime);
            }
    }
}
