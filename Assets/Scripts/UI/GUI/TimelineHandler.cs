using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineHandler : MonoBehaviour
{
    [SerializeField] SpellBook spellBook;
    [SerializeField] GameObject cutSceneRoot;

    [SerializeField] PlayableDirector grimoireDirector;
    [SerializeField] List<NewSpellUnlocked> NewSpellUnlockedUIs;
    [SerializeField] ObjectSignal OnPickupObjectSignal;


    public void StartGrimoireTimeline(List<SpellConfig> spells, Sprite grimoireSprite)
    {
        StartCoroutine(GrimoireCo(spells, grimoireSprite));
    }

    void ResetNewSpellUnlockedUI()
    {
        for (int i = 0; i < NewSpellUnlockedUIs.Count; i++)
        {
            NewSpellUnlockedUIs[i].Text.text = "";
            NewSpellUnlockedUIs[i].Image.enabled = false;
            NewSpellUnlockedUIs[i].Image.sprite = null;
        }
    }

    IEnumerator GrimoireCo(List<SpellConfig> spells, Sprite grimoireSprite)
    {
        cutSceneRoot.SetActive(true);
        ResetNewSpellUnlockedUI();
        for (int i = 0; i < spells.Count; i++)
        {
            NewSpellUnlockedUIs[i].Text.text = spells[i].Name;
            NewSpellUnlockedUIs[i].Image.enabled = true;
            NewSpellUnlockedUIs[i].Image.sprite = spells[i].Icon;
            spellBook.UnlockSpell(spells[i]);
        }
        grimoireDirector.Play();
        OnPickupObjectSignal.Raise(grimoireSprite);
        yield return new WaitForSeconds((float)grimoireDirector.duration);
        OnPickupObjectSignal.Raise(null);
        ResetNewSpellUnlockedUI();
        cutSceneRoot.SetActive(false);
    }

    public void StartBossTimeline(string bossName)
    {

    }
}
