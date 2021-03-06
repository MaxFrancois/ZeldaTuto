﻿using UnityEngine;

public enum InputType
{
    Keyboard = 0,
    XBox = 1
}

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject continueButton;
    [SerializeField] VoidSignal keyboardSignal;
    [SerializeField] VoidSignal controllerSignal;
    //InputType currentInputType;
    public GameObject ContinueButton { get { return continueButton; } }

    void Awake()
    {
        //currentInputType = InputType.Keyboard;  
    }

    void Update()
    {
        //if (Input.anyKeyDown && currentInputType != InputType.Keyboard)
        //    keyboardSignal.Raise();
        //else if (GetGamepadInput() && currentInputType != InputType.XBox)
        //    controllerSignal.Raise();
    }

    public bool GetGamepadInput()
    {
        return Input.GetButtonDown("");
    }
}
