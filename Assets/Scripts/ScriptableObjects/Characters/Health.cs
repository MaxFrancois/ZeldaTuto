using UnityEngine;

[CreateAssetMenu(fileName = "Health", menuName = "Characters/Health")]
public class Health : ScriptableObject
{
    public float MaxHealth;
    public float CurrentHealth;
    public float PassiveHealthRegenSpeed;
}
