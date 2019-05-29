using UnityEngine;

public enum SpellElement
{
    None,
    Fire,
    Ice,
    Air,
    Lightning,
    Earth,
    Dark,
    Light
}

public abstract class SpellConfig : ScriptableObject
{
    [Header("Basics")]
    public string Id;
    public Sprite Icon;
    public Sprite SelectedIcon;
    public string Name;
    public string Description;
    public SpellElement Element;
    public float ManaCost;
    public float PushTime;
    public float PushForce;
    public float Damage;
    public float Cooldown;
    public float LifeTime;
    public bool IsUnlocked;

    public abstract void Cast(Transform source, Vector3 direction);

    public virtual bool CanCast(Transform source, Vector3 direction)
    {
        return true;
    }
}