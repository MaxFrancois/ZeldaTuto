﻿using UnityEngine;
using UnityEngine.UI;

public class Sign : Interactable
{
    public GameObject DialogBox;
    public Text DialogText;
    public string DialogString;

    private CharacterState playerState;

    void Start()
    {
        playerState = GameObject.FindWithTag("Player").GetComponent<CharacterState>();
    }

    protected override void StartInteraction()
    {
        if (DialogBox.activeInHierarchy)
        {
            DialogBox.SetActive(false);
            playerState.MovementState = CharacterMovementState.Idle;
        }
        else
        {
            DialogBox.SetActive(true);
            DialogText.text = DialogString;
            playerState.MovementState = CharacterMovementState.Interacting;
        }
    }
}
