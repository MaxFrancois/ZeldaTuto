using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpellBarManager : MonoBehaviour
{
    public SpellBar SpellBar;
    public Image[] SpellIcons;
    public Image[] SpellCooldownIcons;

    void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        var deltaTime = Time.deltaTime;

        for (int i = SpellBar.Cooldowns.Count - 1; i >= 0; i--)
        {
            if (!SpellBar.Cooldowns[i].HasStarted)
            {
                SpellBar.Cooldowns[i].HasStarted = true;
                SpellCooldownIcons[SpellBar.Cooldowns[i].Index].fillAmount = 1;
            }
            SpellBar.Cooldowns[i].TimeTracker -= deltaTime;
            SpellCooldownIcons[SpellBar.Cooldowns[i].Index].fillAmount -= deltaTime / SpellBar.Cooldowns[i].CooldownTime;
            if (SpellBar.Cooldowns[i].TimeTracker <= 0)
            {
                SpellBar.Cooldowns.Remove(SpellBar.Cooldowns[i]);
            }
        }
    }

    public void UpdateUI()
    {
        for (int i = 0; i < SpellIcons.Length; i++)
        {
            SpellIcons[i].sprite = null;
            SpellCooldownIcons[i].sprite = null;
        }

        for (int i = 0; i < SpellBar.Spells.Count(); i++)
        {
            if (SpellBar.Spells[i] != null)
            {
                SpellIcons[i].sprite = SpellBar.Spells[i].Icon;
                SpellCooldownIcons[i].sprite = SpellBar.Spells[i].Icon;
            }
        }
    }
}
