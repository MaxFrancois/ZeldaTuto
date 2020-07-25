using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLocation", menuName = "SaveData/PlayerLocation")]
public class PlayerLocation : ScriptableObject
{
    public Vector2 Location;
    public Vector3 FacingDirection;
    public bool UseThis;
    public FadePanelSignal FadeFromSignal;
}
