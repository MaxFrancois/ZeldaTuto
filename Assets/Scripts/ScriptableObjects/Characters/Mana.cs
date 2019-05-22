using UnityEngine;

[CreateAssetMenu(fileName = "Mana", menuName = "Characters/Mana")]
public class Mana : ScriptableObject
{
    public float MaxMana;
    public float CurrentMana;
    public float PassiveManaRegenSpeed;
}
