using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushTrap : MonoBehaviour
{
    public float StartDelay;
    public float TimeBetweenPushes;
    public float timeSincePush;
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        timeSincePush = StartDelay;
    }

    void Update()
    {
        if (timeSincePush > 0)
            timeSincePush -= Time.deltaTime;
        else
        {
            animator.SetTrigger("Act");
            timeSincePush = TimeBetweenPushes;
        }
    }   
}
