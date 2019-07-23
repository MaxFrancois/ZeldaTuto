using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class NewSpellUnlocked
{
    public TextMeshProUGUI Text;
    public Image Image;
}

public class Grimoire : Interactable
{
    [SerializeField] List<SpellConfig> SpellsToUnlock;
    [SerializeField] SpellBook SpellBook;

    [SerializeField] PlayableDirector TimelineDirector;
    [SerializeField] List<NewSpellUnlocked> NewSpellUnlockedUIs;
    [SerializeField] ObjectSignal OnPickupObjectSignal;

    [SerializeField] TimelineSignal timelineSignal;

    protected override void StartInteraction()
    {
        //StartCoroutine(UnlockSpells());
        canInteract = false;
        Context.Raise(false);
        var renderer = GetComponent<SpriteRenderer>();
        var sprite = renderer.sprite;
        timelineSignal.Raise(SpellsToUnlock, sprite);
        renderer.enabled = false;
        Data.CanBeInteractedWith = false;
        if (Data.HideWhenCannotInteract)
            gameObject.SetActive(false);
    }

    //IEnumerator UnlockSpells()
    //{
    //    canInteract = false;
    //    Context.Raise(false);
    //    ResetNewSpellUnlockedUI();
    //    for (int i = 0; i < SpellsToUnlock.Count; i++)
    //    {
    //        NewSpellUnlockedUIs[i].Text.text = SpellsToUnlock[i].Name;
    //        NewSpellUnlockedUIs[i].Image.enabled = true;
    //        NewSpellUnlockedUIs[i].Image.sprite = SpellsToUnlock[i].Icon;
    //        SpellBook.UnlockSpell(SpellsToUnlock[i]);
    //    }
    //    TimelineDirector.Play();
    //    var renderer = GetComponent<SpriteRenderer>();
    //    OnPickupObjectSignal.Raise(renderer.sprite);
    //    renderer.enabled = false;
    //    yield return new WaitForSeconds((float)TimelineDirector.duration);
    //    OnPickupObjectSignal.Raise(null);
    //    Data.CanBeInteractedWith = false;
    //    ResetNewSpellUnlockedUI();
    //    if (Data.HideWhenCannotInteract)
    //        gameObject.SetActive(false);
    //}

    //void ResetNewSpellUnlockedUI()
    //{
    //    for (int i = 0; i < NewSpellUnlockedUIs.Count; i++)
    //    {
    //        NewSpellUnlockedUIs[i].Text.text = "";
    //        NewSpellUnlockedUIs[i].Image.enabled = false;
    //        NewSpellUnlockedUIs[i].Image.sprite = null;
    //    }
    //}
}
