using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Conversation", menuName = "Dialogs/Conversation")]
public class Conversation : ScriptableObject
{
    public List<Dialog> Dialogs;
    public bool OpenBubbleOnStart;
    public bool CloseBubbleOnFinish;
}
