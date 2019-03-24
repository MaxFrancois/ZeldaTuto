using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SpellDetailsMenu : MonoBehaviour
{
    public Text Name;
    public Text Description;
    public GameObject DemoPlayer;

    public void Initialize(Button btn)
    {
        var spell = btn.gameObject.GetComponent<SpellContainer>().Spell;
        Name.text = spell.Name;
        Description.text = spell.Description;
        DemoPlayer.GetComponent<Animator>().runtimeAnimatorController = 
            Resources.Load(spell.Name) as RuntimeAnimatorController;
    }
}
