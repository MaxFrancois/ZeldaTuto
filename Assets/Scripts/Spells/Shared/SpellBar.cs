using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Cooldown
{
    public SpellConfig Spell;
    public float TimeTracker;
    public Image CooldownImage;
    public float CooldownTime;
}

public class SpellBar : MonoBehaviour
{
    public int MaxQuantity;
    public List<Cooldown> Cooldowns = new List<Cooldown>();
    [Header("Spells")]
    public List<Image> CooldownImages;
    public List<SpellConfig> Spells;
    public VoidSignal SpellChangedSignal;
    public SpellConfig Ultimate;

    public void AddSpell(SpellConfig spell)
    {
        if (!Spells.Contains(spell) && Spells.Count < MaxQuantity)
            Spells.Add(spell);
    }

    public void RemoveSpell(SpellConfig spell)
    {
        if (Spells.Contains(spell))
            Spells.Remove(spell);
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
            if (spellIndex < MaxQuantity && spellIndex >= 0)
                if (!Cooldowns.Any(c => c.Spell.Name == Spells[spellIndex].Name))
                {
                    var spell = Spells[spellIndex];//, source.position, Quaternion.identity);
                    if (Spells[spellIndex].Cooldown > 0)
                    {
                        CooldownImages[spellIndex].fillAmount = 1;
                        Cooldowns.Add(new Cooldown()
                        {
                            TimeTracker = Spells[spellIndex].Cooldown,
                            CooldownTime = Spells[spellIndex].Cooldown,
                            Spell = Spells[spellIndex],
                            CooldownImage = CooldownImages[spellIndex]
                        });
                    }
                    spell.Cast(source, direction);
                }
        }
    }

    private void Update()
    {
        var deltaTime = Time.deltaTime;

        for (int i = Cooldowns.Count - 1; i >= 0; i--)
        {
            Cooldowns[i].TimeTracker -= deltaTime;
            Cooldowns[i].CooldownImage.fillAmount -= deltaTime / Cooldowns[i].CooldownTime;
            if (Cooldowns[i].TimeTracker <= 0)
            {
                Cooldowns.Remove(Cooldowns[i]);
            }
        }
    }

    public List<SpellConfig> GetCurrentSpells()
    {
        return Spells;
    }
}
