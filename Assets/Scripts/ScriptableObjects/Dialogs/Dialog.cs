using UnityEngine;

public enum DialogSpeakerRelation
{
    Friendly = 0,
    Neutral = 1,
    Aggressive = 2
}

public enum DialogAlignment
{
    Left,
    Right
}

[CreateAssetMenu(fileName = "Dialog", menuName = "Dialogs/Dialog")]
public class Dialog : ScriptableObject
{
    public string Text;
    public Sprite CharacterIcon;
    public string CharacterName;
    public DialogSpeakerRelation SpeakerRelation;
    public DialogAlignment Alignment;
}
