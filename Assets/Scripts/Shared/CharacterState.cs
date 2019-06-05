using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CharacterMovementState
{
    Idle,
    Walking,
    Interacting,
    Attacking,
    Stunned,
    //Rooted,
    Dashing,
    Jumping,
    Falling,
    Dead
}

public class StatusEffectRendering
{
    public StatusEffect StatusEffect;
    public GameObject RenderingIcon;
}

public class CharacterState : MonoBehaviour
{
    //[HideInInspector]
    public CharacterMovementState _state;
    public CharacterMovementState MovementState
    {
        get
        {
            return _state;
        }
        set
        {
            if (_state != value)
                _state = value;
        }
    }
    public GameObject StatusEffectDisplayArea;
    public GameObject StatusEffectIconPrefab;
    public float SpaceBetweenIcons;
    protected List<StatusEffectRendering> StatusEffects;

    void Awake()
    {
        MovementState = CharacterMovementState.Idle;
        StatusEffects = new List<StatusEffectRendering>();
    }

    void Update()
    {
        if (StatusEffects != null)
        {
            foreach (var effect in StatusEffects)
            {
                effect.StatusEffect.Update();
            }
        }
    }

    public void AddStatusEffect(StatusEffect se)
    {
        //var iconPosition = new Vector3(StatusEffectDisplayArea.transform.position.x + (StatusEffects.Count * 0.2f), StatusEffectDisplayArea.transform.position.y, 0);
        var iconPosition = new Vector3(StatusEffects.Count * SpaceBetweenIcons, 0.1f, 0);
        var icon = Instantiate(StatusEffectIconPrefab, iconPosition, Quaternion.identity);
        icon.transform.SetParent(StatusEffectDisplayArea.transform);
        icon.transform.localPosition = iconPosition;
        icon.GetComponent<SpriteRenderer>().sprite = se.Config.Icon;
        StatusEffects.Add(new StatusEffectRendering() { StatusEffect = se, RenderingIcon = icon });
    }

    public void RemoveStatusEffect(StatusEffect se)
    {
        var effectToRemove = StatusEffects.First(c => c.StatusEffect.Config.Name == se.Config.Name);
        Destroy(effectToRemove.RenderingIcon);
        StatusEffects.Remove(effectToRemove);
        foreach (var effect in StatusEffects)
        {
            effect.RenderingIcon.transform.localPosition = new Vector3(effect.RenderingIcon.transform.localPosition.x - SpaceBetweenIcons, effect.RenderingIcon.transform.localPosition.y, 0);
        }
    }
}
