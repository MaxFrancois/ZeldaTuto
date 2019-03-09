using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cooldown
{
    public Spell Spell;
    public float TimeTracker;
}

public class SpellBar : MonoBehaviour
{
    public List<Cooldown> Cooldowns = new List<Cooldown>();
    public List<Spell> Spells;
    public int MaxQuantity;
    public FloatSignal SpendManaSignal;

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
            if (Spells[spellIndex] != null && !Cooldowns.Any(c => c.Spell.Name == Spells[spellIndex].Name))
            {
                SpendManaSignal.Raise(Spells[spellIndex].ManaCost);
                var spell = Instantiate(Spells[spellIndex], source.position, Quaternion.identity);
                if (Spells[spellIndex].Cooldown > 0)
                    Cooldowns.Add(new Cooldown() { TimeTracker = Spells[spellIndex].Cooldown, Spell = Spells[spellIndex] });
                spell.Cast(source, direction);
            }
    }

    private void Update()
    {
        var deltaTime = Time.deltaTime;

        for (int i = Cooldowns.Count - 1; i >= 0; i--)
        {
            Cooldowns[i].TimeTracker -= deltaTime;
            if (Cooldowns[i].TimeTracker <= 0)
            {
                Cooldowns.Remove(Cooldowns[i]);
            }
        }
    }
}
