using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBoss : EnemyBase
{
    [Header("Basic")]
    protected float currentHealth;
    public FloatValue MaxHealth;
    public string Name;
    public Vector2 HomePosition;
    protected Transform Target;
    public LootTable LootTable;
    protected Animator animator;
    protected bool isDead;
    public FloatSignal HitSignal;
    public VoidSignal DeadSignal;
}

