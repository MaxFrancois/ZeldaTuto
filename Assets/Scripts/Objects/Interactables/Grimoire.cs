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
    [SerializeField] BoolValue HasBeenPickedup;

    [SerializeField] PlayableDirector TimelineDirector;
    [SerializeField] List<NewSpellUnlocked> NewSpellUnlockedUIs;
    [SerializeField] ObjectSignal OnPickupObjectSignal;
    bool canInteract;

    void Awake()
    {
        canInteract = true;
        #if UNITY_EDITOR
#else
            if (HasBeenPickedup.RuntimeValue)
                gameObject.SetActive(false);
#endif
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact") && IsActive)
        {
            StartCoroutine(UnlockSpells());
        }
    }

    IEnumerator UnlockSpells()
    {
        canInteract = false;
        Context.Raise(false);
        ResetNewSpellUnlockedUI();
        for (int i = 0; i < SpellsToUnlock.Count; i++)
        {
            NewSpellUnlockedUIs[i].Text.text = SpellsToUnlock[i].Name;
            NewSpellUnlockedUIs[i].Image.enabled = true;
            NewSpellUnlockedUIs[i].Image.sprite = SpellsToUnlock[i].Icon;
            SpellBook.UnlockSpell(SpellsToUnlock[i]);
        }
        TimelineDirector.Play();
        var renderer = GetComponent<SpriteRenderer>();
        OnPickupObjectSignal.Raise(renderer.sprite);
        renderer.enabled = false;
        yield return new WaitForSeconds((float)TimelineDirector.duration);
        OnPickupObjectSignal.Raise(null);
        HasBeenPickedup.RuntimeValue = true;
        ResetNewSpellUnlockedUI();
        gameObject.SetActive(false);
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
    
    protected override bool CanInteract()
    {
        return canInteract;
    }
}
