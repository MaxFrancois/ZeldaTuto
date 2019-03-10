using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBarManager : MonoBehaviour
{
    public SpellBar SpellBar;
    public Image[] SpellIcons;
    public Image[] SpellCooldownIcons;

    private void Awake()
    {
        foreach (var img in SpellCooldownIcons)
            img.sprite = null;
        UpdateUI();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < SpellBar.Spells.Count; i++)
        {
            if (SpellIcons[i] != null && SpellBar.Spells[i] != null)
            {
                SpellIcons[i].sprite = SpellBar.Spells[i].Icon;
                SpellCooldownIcons[i].sprite = SpellBar.Spells[i].Icon;
            }
        }
    }
}
