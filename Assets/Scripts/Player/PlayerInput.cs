using System;
using System.Linq;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [HideInInspector]
    public Vector3 CurrentFacingDirection;
    [HideInInspector]
    public Vector3 CurrentInputDirection;
    protected CharacterState PlayerState;

    //public event Action OnInterract = delegate { };
    public event Action OnJump = delegate { };
    public event Action OnAttack = delegate { };
    public event Action<int> OnCastSpell = delegate { };

    void Start()
    {
        PlayerState = GetComponent<CharacterState>();
    }

    private bool CanAct()
    {
        if ((PlayerState.MovementState != CharacterMovementState.Idle && 
            PlayerState.MovementState != CharacterMovementState.Walking) ||
            MenuManager.IsPaused || MenuManager.RecentlyUnpaused)
            return false;
        return true;
    }

    public Interactable CanInteract()
    {
        var closeColliders = Physics2D.OverlapCircleAll(transform.position, 1f).Where(c => c.isTrigger);
        float angleToClosestInteractable = Mathf.Infinity;
        Interactable closestInteractable = null;
        foreach (var collider in closeColliders)
        {
            Interactable interactable = collider.GetComponent<Interactable>();
            if (interactable == null) continue;

            Vector3 directionToInteractable = collider.transform.position - transform.position;
            var angleToInteractable = Vector3.Angle(CurrentFacingDirection, directionToInteractable);
            if (angleToInteractable < 40)
                if (angleToInteractable < angleToClosestInteractable)
                {
                    angleToClosestInteractable = angleToInteractable;
                    closestInteractable = interactable;
                }
        }
        return closestInteractable;
    }

    void Update()
    {
        if (!CanAct()) return;

        CurrentInputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        if (CurrentInputDirection != Vector3.zero)
            CurrentFacingDirection = CurrentInputDirection;
        if (Input.GetButtonDown("Attack"))
        {
            OnAttack();
        }
        else if (Input.GetButtonDown("Ultimate"))
        {
            OnCastSpell(-1);
        }
        else if (Input.GetButtonDown("Spell 0"))
        {
            if (!CanInteract())
                OnCastSpell(0);
            //else
            //    OnInterract();
        }
        else if (Input.GetButtonDown("Spell 1"))
        {
            OnCastSpell(1);
        }
        else if (Input.GetButtonDown("Spell 2"))
        {
            OnCastSpell(2);
        }
        else if (Input.GetButtonDown("Spell 3"))
        {
            OnCastSpell(3);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("StartingJump");
            OnJump();
        }
    }
}
