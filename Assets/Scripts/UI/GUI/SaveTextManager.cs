using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTextManager : MonoBehaviour
{
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ShowSaveText()
    {
        animator.SetTrigger("Save");
    }
}
