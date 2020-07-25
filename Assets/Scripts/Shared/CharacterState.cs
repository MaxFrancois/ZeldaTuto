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
            {
                var check = false;
                if (_state == CharacterMovementState.Dashing && isPlayer)
                    check = true;
                _state = value;
                if (check) CheckForHoles();
            }
        }
    }
    public GameObject StatusEffectDisplayArea;
    public GameObject StatusEffectIconPrefab;
    public float SpaceBetweenIcons;
    protected List<StatusEffectRendering> StatusEffects;
    protected Collider2D CurrentlyOverThisHole;
    bool isPlayer;
    void Awake()
    {
        MovementState = CharacterMovementState.Idle;
        StatusEffects = new List<StatusEffectRendering>();
        isPlayer = gameObject.CompareTag("Player");
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
        if (StatusEffectIconPrefab)
        {
            //var iconPosition = new Vector3(StatusEffectDisplayArea.transform.position.x + (StatusEffects.Count * 0.2f), StatusEffectDisplayArea.transform.position.y, 0);
            var iconPosition = new Vector3(StatusEffects.Count * SpaceBetweenIcons, 0.1f, 0);
            var icon = Instantiate(StatusEffectIconPrefab, iconPosition, Quaternion.identity);
            icon.transform.SetParent(StatusEffectDisplayArea.transform);
            icon.transform.localPosition = iconPosition;
            icon.GetComponent<SpriteRenderer>().sprite = se.Config.Icon;
            StatusEffects.Add(new StatusEffectRendering() { StatusEffect = se, RenderingIcon = icon });
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<FloorHole>())
            CurrentlyOverThisHole = collision;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == CurrentlyOverThisHole)
            CurrentlyOverThisHole = null;
    }

    void CheckForHoles()
    {
        if (CurrentlyOverThisHole)
            CurrentlyOverThisHole.GetComponent<FloorHole>().TriggerFall(gameObject);
            //GetComponent<PlayerMovement>().TriggerFall(CurrentlyOverThisHole.GetComponent<FloorSpike>().Damage, CurrentlyOverThisHole.bounds.center);
    }
}
