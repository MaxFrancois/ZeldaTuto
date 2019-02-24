﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextClue : MonoBehaviour
{
    public GameObject Clue;
    public bool IsActive = false;

    public void ChangeActive()
    {
        IsActive = !IsActive;
        Clue.SetActive(IsActive);
    }
}
