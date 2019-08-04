using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Cooldown
{
    public SpellConfig Spell;
    public float TimeTracker;
    //public Image CooldownImage;
    public int Index;
    public bool HasStarted;
    public float CooldownTime;
}

[System.Serializable]
public class BoundSpells {

    public BoundSpells()
    {
        ResetSpells();
    }
    public List<SpellConfig> Spells;
    const int numberOfSpells = 4;
    public void ResetSpells()
    {
        Spells = new List<SpellConfig>();
        for (int i = 0; i < numberOfSpells; i++)
            Spells.Add(null);
    }
}

[CreateAssetMenu]
public class SpellBar : ScriptableObject
{
    [HideInInspector]
    public List<Cooldown> Cooldowns = new List<Cooldown>();
    public List<SpellConfig> Spells;
    public SpellConfig Ultimate;
    [SerializeField] int MaxSpells;
    [SerializeField] VoidSignal SpellChangedSignal;

    public void Initialize(List<SpellConfig> spells)
    {
        Spells = new List<SpellConfig>(MaxSpells);
        if (spells.Count == MaxSpells)
        {
            foreach (var s in spells)
                Spells.Add(s);
            SpellChangedSignal.Raise();
        }
    }

    public void CastUltimate(Transform source, Vector3 direction)
    {
        Ultimate.Cast(source, direction);
    }

    public void ChangeSpell(int idx, SpellConfig newSpell)
    {
        Spells[idx] = newSpell;
        SpellChangedSignal.Raise();
    }

    public void CastSpell(int spellIndex, Transform source, Vector3 direction)
    {
        if (Spells[spellIndex] != null && Spells[spellIndex].CanCast(source, direction))
        {
            if (spellIndex < MaxSpells && spellIndex >= 0)
                if (!Cooldowns.Any(c => c.Spell.Name == Spells[spellIndex].Name))
                {
                    var spell = Spells[spellIndex];//, source.position, Quaternion.identity);
                    if (Spells[spellIndex].Cooldown > 0)
                    {
                        Cooldowns.Add(new Cooldown()
                        {
                            TimeTracker = Spells[spellIndex].Cooldown,
                            CooldownTime = Spells[spellIndex].Cooldown,
                            Spell = Spells[spellIndex],
                           HasStarted = false,
                           Index = spellIndex
                        });
                    }
                    spell.Cast(source, direction);
                }
        }
    }
}
