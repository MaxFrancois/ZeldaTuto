using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

[System.Serializable]
public class SpellBookMenuTab
{
    public Image TabImage;
    public SpellElement Element;
    [HideInInspector]
    public bool Selected;
}

public class SpellBookMenu : MonoBehaviour
{
    [Header("Tabs")]
    [SerializeField] List<SpellBookMenuTab> Tabs;
    [SerializeField] float TabXIndex;
    [SerializeField] float TabXOffset;
    [SerializeField] GameObject tabNavigationRightIcon;
    List<SpellBookMenuTab> activeTabs;
    int selectedTabIndex;

    [Header("Bound Buttons")]
    [SerializeField] GameObject DefaultSelectedButton;
    [SerializeField] List<Button> BoundButtons;
    Button[][] ButtonLinksArray;
    List<Button> SpellButtons;

    [Header("Spell Grid")]
    [SerializeField] GameObject SpellListContainer;
    [SerializeField] Button SpellButtonPrefab;
    [SerializeField] float SpellIconsOffset;
    [SerializeField] float SpellIconsXIndex;
    [SerializeField] Sprite EmptySelectionSquare;
    [SerializeField] int SpellsPerLine;
    [SerializeField] float NumberOfLines;

    [Header("Navigation")]
    [SerializeField] GameObject RestMenu;

    int selectedButtonIndex;
    bool boundSpellSelected = false;
    [SerializeField] SpellBar PlayerSpellBar;
    [SerializeField] SpellBook SpellBook;
    [SerializeField] SpellDetailsMenu SpellDetailsMenu;

    EventSystem _eventSystem;
    EventSystem eventSystem
    {
        get
        {
            if (_eventSystem == null)
                _eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
            return _eventSystem;
        }
    }

    void OnEnable()
    {
        selectedButtonIndex = 0;
        boundSpellSelected = false;
        selectedTabIndex = 0;
        InitializeTabs();
        SetTabSelected(selectedTabIndex, true);
        //setup controller buttons at the top
        for (int i = 0; i < PlayerSpellBar.Spells.Count; i++)
        {
            if (PlayerSpellBar.Spells[i] != null)
            {
                UpdateButton(BoundButtons[i], PlayerSpellBar.Spells[i]);
            }
        }

        SetupButtons();

        //TODO: figure out how to pre select the first button
        BoundButtons[0].Select();
        StartCoroutine(SetDefaultSelectedSpell());
    }

    IEnumerator SetDefaultSelectedSpell()
    {
        yield return new WaitForSeconds(0.2f);
        eventSystem.SetSelectedGameObject(DefaultSelectedButton);
    }

    void InitializeTabs()
    {
        activeTabs = new List<SpellBookMenuTab>();
        foreach (var tab in Tabs)
        {
            if (tab.Element == SpellElement.None || SpellBook.GetUnlockedSpellsByElement(tab.Element).Count > 0)
            {
                var rect = tab.TabImage.gameObject.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(TabXOffset + TabXIndex * activeTabs.Count, rect.anchoredPosition.y);
                activeTabs.Add(tab);
                tab.TabImage.gameObject.SetActive(true);
            }
            else
                tab.TabImage.gameObject.SetActive(false);
        }
        var rb = tabNavigationRightIcon.GetComponent<RectTransform>();
        rb.anchoredPosition = new Vector2(TabXOffset + TabXIndex * activeTabs.Count, rb.anchoredPosition.y);

    }

    // Go through the available spells in the spellbook
    // Create a button for each, link it to other buttons around
    // And bind the proper spellconfig to it
    private void SetupButtons()
    {
        int spellIndex = 0;
        var unlockedSpells = new List<SpellConfig>();
        var selectedTabElement = activeTabs.First(c => c.Selected).Element;
        if (selectedTabElement == SpellElement.None)
            unlockedSpells = SpellBook.GetAllUnlockedSpells();
        else
            unlockedSpells = SpellBook.GetUnlockedSpellsByElement(selectedTabElement);
        NumberOfLines = Mathf.Ceil((float)unlockedSpells.Count / (float)SpellsPerLine);
        if (SpellButtons != null)
            for (int i = SpellButtons.Count - 1; i >= 0; i--)
                if (SpellButtons[i] != null)
                    Destroy(SpellButtons[i].gameObject);
        SpellButtons = new List<Button>();
        int currentColumn = 0;
        int currentLine = 0;
        ButtonLinksArray = new Button[(int)NumberOfLines][];
        for (int i = 0; i < NumberOfLines; i++)
            ButtonLinksArray[i] = new Button[SpellsPerLine];
        while (spellIndex < unlockedSpells.Count)
        {
            var xIndex = SpellIconsOffset + currentColumn * SpellIconsXIndex;
            var yIndex = -SpellIconsOffset + currentLine * -SpellIconsXIndex;
            var buttonPosition = new Vector2(xIndex, yIndex);
            var b = Instantiate(SpellButtonPrefab, buttonPosition, Quaternion.identity, SpellListContainer.transform);
            b.gameObject.GetComponent<RectTransform>().anchoredPosition = buttonPosition;
            ButtonLinksArray[currentLine][currentColumn] = b;
            currentColumn++;
            if (currentColumn >= SpellsPerLine)
            {
                currentLine++;
                currentColumn = 0;
            }

            SpellButtons.Add(b);
            spellIndex++;
        }
        LinkButtons();
        for (int i = 0; i < SpellButtons.Count; i++)
            UpdateButton(SpellButtons[i], unlockedSpells[i], i);
    }

    // Create the link between a button and those around it
    private void LinkButtons()
    {
        for (int j = 0; j < NumberOfLines; j++)
        {
            for (int i = 0; i < SpellsPerLine; i++)
            {
                if (ButtonLinksArray[j][i])
                {
                    var nav = new Navigation();

                    int currentIndex = i - 1;
                    Button leftButton = null;
                    while (leftButton == null)
                    {
                        if (currentIndex < 0)
                            currentIndex = SpellsPerLine - 1;
                        if (ButtonLinksArray[j][currentIndex] != null)
                            leftButton = ButtonLinksArray[j][currentIndex];
                        currentIndex--;
                    }

                    currentIndex = i + 1;
                    Button rightButton = null;
                    while (rightButton == null)
                    {
                        if (currentIndex >= SpellsPerLine)
                            currentIndex = 0;
                        if (ButtonLinksArray[j][currentIndex] != null)
                            rightButton = ButtonLinksArray[j][currentIndex];
                        currentIndex++;
                    }


                    currentIndex = j - 1;
                    Button upButton = null;
                    while (upButton == null)
                    {
                        if (currentIndex < 0)
                            currentIndex = (int)NumberOfLines - 1;
                        if (ButtonLinksArray[currentIndex][i] != null)
                            upButton = ButtonLinksArray[currentIndex][i];
                        currentIndex--;
                    }

                    currentIndex = j + 1;
                    Button downButton = null;
                    while (downButton == null)
                    {
                        if (currentIndex >= (int)NumberOfLines)
                            currentIndex = 0;
                        if (ButtonLinksArray[currentIndex][i] != null)
                            downButton = ButtonLinksArray[currentIndex][i];
                        currentIndex++;
                    }

                    nav.mode = Navigation.Mode.Explicit;
                    nav.selectOnLeft = leftButton;
                    nav.selectOnRight = rightButton;
                    nav.selectOnUp = upButton;
                    nav.selectOnDown = downButton;
                    ButtonLinksArray[j][i].navigation = nav;
                }
            }
        }
    }

    public void SetSelectedButtonIndex(int idx)
    {
        Debug.Log("Selected button " + idx);
        selectedButtonIndex = idx;
        boundSpellSelected = true;
        eventSystem.SetSelectedGameObject(SpellButtons[0].gameObject);
    }

    public void SetSelectedSpell(int idx)
    {
        var newSpell = SpellButtons[idx].gameObject.GetComponent<SpellContainer>().Spell;
        if (newSpell != null)
        {
            SpellDetailsMenu.gameObject.SetActive(false);
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

    private void UpdateButton(Button btn, SpellConfig newSpell, int index = -1)
    {
        if (newSpell != null)
        {
            btn.image.sprite = newSpell.Icon;
            var ss = btn.spriteState;
            ss.highlightedSprite = newSpell.SelectedIcon;
            ss.selectedSprite = newSpell.SelectedIcon;
            ss.pressedSprite = newSpell.SelectedIcon;
            btn.spriteState = ss;
            if (index != -1)
            {
                btn.onClick.AddListener(delegate { SetSelectedSpell(index); });
            }
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
        if (Input.GetButtonDown("Left Bumper"))
        {
            SwitchTab(-1);
            SetupButtons();
        }
        if (Input.GetButtonDown("Right Bumper"))
        {
            SwitchTab(1);
            SetupButtons();
        }
        if (!boundSpellSelected)
        {
            if (Input.GetButtonDown("Spell 3"))
            {
                Debug.Log("Returning to rest menu");
                BackToRestMenu();
            }
        }
        else
        {
            if (Input.GetButtonDown("Spell 2"))
            {
                SpellDetailsMenu.gameObject.SetActive(true);
                var btn = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
                SpellDetailsMenu.Initialize(btn);

            }
            if (Input.GetButtonDown("Spell 3"))
            {
                SpellDetailsMenu.gameObject.SetActive(false);
                Debug.Log("Returning to button select");
                boundSpellSelected = false;
                eventSystem.SetSelectedGameObject(BoundButtons[selectedButtonIndex].gameObject);
            }
        }
    }

    private void SwitchTab(int index)
    {
        SetTabSelected(selectedTabIndex, false);
        selectedTabIndex += index;
        if (selectedTabIndex < 0)
            selectedTabIndex = activeTabs.Count - 1;
        else if (selectedTabIndex > activeTabs.Count - 1)
            selectedTabIndex = 0;
        SetTabSelected(selectedTabIndex, true);

        SpellDetailsMenu.gameObject.SetActive(false);
        boundSpellSelected = false;
        eventSystem.SetSelectedGameObject(BoundButtons[selectedButtonIndex].gameObject);
    }

    private void SetTabSelected(int index, bool selected)
    {
        var color = activeTabs[index].TabImage.color;
        activeTabs[index].TabImage.color = new Color(color.r, color.g, color.b, selected ? 1 : 0.5f);
        activeTabs[index].Selected = selected;
    }

    private void BackToRestMenu()
    {
        SetTabSelected(selectedTabIndex, false);
        for (int i = SpellButtons.Count - 1; i >= 0; i--)
            Destroy(SpellButtons[i].gameObject);
        eventSystem.SetSelectedGameObject(null);
        RestMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
