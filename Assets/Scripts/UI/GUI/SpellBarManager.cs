using UnityEngine;
using UnityEngine.UI;

public class SpellBarManager : MonoBehaviour
{
    public SpellBar SpellBar;
    public Image[] SpellIcons;
    public Image[] SpellCooldownIcons;

    private void Awake()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < SpellIcons.Length; i++)
        {
            SpellIcons[i].sprite = null;
            SpellCooldownIcons[i].sprite = null;
        }

        for (int i = 0; i < SpellBar.Spells.Count; i++)
        {
            if (SpellBar.Spells[i] != null)
            {
                SpellIcons[i].sprite = SpellBar.Spells[i].Icon;
                SpellCooldownIcons[i].sprite = SpellBar.Spells[i].Icon;
            }
        }
    }
}
