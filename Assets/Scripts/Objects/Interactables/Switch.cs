using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public BoolValue StoredActiveValue;
    public bool IsActivated;
    public Sprite ActivatedSprite;
    private SpriteRenderer spriteRenderer;
    public Door Door;
    
    void Start()
    {
        IsActivated = StoredActiveValue.RuntimeValue;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (IsActivated)
        {
            ActivateSwitch();
        }
    }

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (!IsActivated && collidedObject.CompareTag("Player")) {
            ActivateSwitch();
        }
    }

    private void ActivateSwitch()
    {
        IsActivated = true;
        StoredActiveValue.RuntimeValue = IsActivated;
        Door.Open();
        spriteRenderer.sprite = ActivatedSprite;
    }
}
