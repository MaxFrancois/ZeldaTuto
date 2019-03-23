using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellBookMenu : MonoBehaviour
{
    public List<Button> BoundButtons;
    public List<Button> SpellButtons;
    public Sprite EmptySelectionSquare;
    public GameObject RestMenu;
    public int NbOfSpellsPerLine;
    public int NbOfSpellsPerColumn;
    private int selectedButtonIndex;
    private bool boundSpellSelected = false;
    public EventSystem eventSystem;
    public GameObject DefaultSelectedObject;
    public GameObject DefaultSelectedSpell;
    private bool buttonSelected;
    public SpellBar PlayerSpellBar;
    public SpellBook SpellBook;

    private void OnEnable()
    {
        buttonSelected = false;
        selectedButtonIndex = 0;
        boundSpellSelected = false;
        for (int i = 0; i < PlayerSpellBar.Spells.Count; i++)
        {
            if (PlayerSpellBar.Spells[i] != null)
            {
                UpdateButton(BoundButtons[i], PlayerSpellBar.Spells[i]);
            }
        }

        int spellCount = 0;
        while (spellCount < SpellBook.Spells.Count)
        {
            if (SpellBook.Spells[spellCount].IsUnlocked)
            {
                UpdateButton(SpellButtons[spellCount], SpellBook.Spells[spellCount]);
            }
            else
            {
                SpellButtons[spellCount].gameObject.SetActive(false);
            }
            spellCount++;
        }

        while (spellCount < SpellButtons.Count)
        {
            SpellButtons[spellCount].gameObject.SetActive(false);
            spellCount++;
        }

        eventSystem.SetSelectedGameObject(DefaultSelectedObject);
    }

    public void SetSelectedButtonIndex(int idx)
    {
        Debug.Log("Selected button " + idx);
        selectedButtonIndex = idx;
        boundSpellSelected = true;
        eventSystem.SetSelectedGameObject(DefaultSelectedSpell);
    }

    public void SetSelectedSpell(int idx)
    {
        var newSpell = SpellButtons[idx].gameObject.GetComponent<SpellContainer>().Spell;
        if (newSpell != null)
        {
            Debug.Log("setting spell " + idx + " to button " + selectedButtonIndex);
            var alreadyBoundIdx = -1;
            for (int i = 0; i < BoundButtons.Count; i++)
            {
                var alreadyBoundSpell = BoundButtons[i].gameObject.GetComponent<SpellContainer>().Spell;
                if (i != selectedButtonIndex && alreadyBoundSpell != null && alreadyBoundSpell == newSpell)
                {
                    alreadyBoundIdx = i;
                    break;
                }
            }
            if (alreadyBoundIdx == -1)
            {
                PlayerSpellBar.ChangeSpell(selectedButtonIndex, newSpell);
                UpdateButton(BoundButtons[selectedButtonIndex], newSpell);
            }
            else
            {
                var spellToMove = BoundButtons[selectedButtonIndex].gameObject.GetComponent<SpellContainer>().Spell;
                SwapButtons(alreadyBoundIdx, selectedButtonIndex, spellToMove, newSpell);
            }
            boundSpellSelected = false;
        }
        else
        {
            Debug.Log("couldnt find spell container " + idx);
        }
        eventSystem.SetSelectedGameObject(BoundButtons[selectedButtonIndex].gameObject);
    }

    private void SwapButtons(int idx1, int idx2, SpellConfig newSpell, SpellConfig spellToMove)
    {
        PlayerSpellBar.ChangeSpell(idx1, newSpell);
        UpdateButton(BoundButtons[idx1], newSpell);

        PlayerSpellBar.ChangeSpell(idx2, spellToMove);
        UpdateButton(BoundButtons[idx2], spellToMove);
    }

    private void UpdateButton(Button btn, SpellConfig newSpell)
    {
        if (newSpell != null)
        {
            btn.image.sprite = newSpell.Icon;
            var ss = btn.spriteState;
            ss.highlightedSprite = newSpell.SelectedIcon;
            ss.selectedSprite = newSpell.SelectedIcon;
            ss.pressedSprite = newSpell.SelectedIcon;
            btn.spriteState = ss;
            btn.gameObject.GetComponent<SpellContainer>().Spell = newSpell;
        }
        else
        {
            btn.image.sprite = null;
            var ss = btn.spriteState;
            ss.highlightedSprite = EmptySelectionSquare;
            ss.selectedSprite = EmptySelectionSquare;
            ss.pressedSprite = EmptySelectionSquare;
            btn.spriteState = ss;
            btn.gameObject.GetComponent<SpellContainer>().Spell = null;
        }
    }

    void Update()
    {
        if (!boundSpellSelected)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 && !buttonSelected)
            {
                eventSystem.SetSelectedGameObject(DefaultSelectedObject);
                buttonSelected = true;
            }
            if (Input.GetButtonDown("Spell 3"))
            {
                Debug.Log("Returning to rest menu");
                BackToRestMenu();
            }
        }
        else
        {
            if (Input.GetButtonDown("Spell 3"))
            {
                Debug.Log("Returning to button select");
                boundSpellSelected = false;
                eventSystem.SetSelectedGameObject(BoundButtons[selectedButtonIndex].gameObject);
            }
        }
    }

    private void BackToRestMenu()
    {
        eventSystem.SetSelectedGameObject(null);
        RestMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
