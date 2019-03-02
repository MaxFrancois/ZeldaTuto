﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Walking,
    Attacking,
    Staggered
}

public class Enemy : MonoBehaviour
{
    public EnemyState CurrentState;
    public float CurrentHealth;
    public FloatValue MaxHealth;
    public string Name;
    public int BaseAttack;
    public float MoveSpeed;
    public GameObject DeathAnimation;
    public Vector2 HomePosition;
    [Header("Death Signals")]
    public CustomSignal RoomSignal;
    public LootTable LootTable;

    private void Awake()
    {
        transform.position = HomePosition;
        CurrentHealth = MaxHealth.InitialValue;
    }

    private void OnEnable()
    {
        transform.position = HomePosition;
        CurrentHealth = MaxHealth.InitialValue;
    }

    public void Knock(Rigidbody2D body, float pushTime, float damage)
    {
        if (transform.gameObject.activeInHierarchy)
            StartCoroutine(Knockback(body, pushTime));
        UpdateHealth(damage);
    }

    private void UpdateHealth(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            if (DeathAnimation != null) {
                var deathAnim = Instantiate(DeathAnimation, transform.position, Quaternion.identity);
                Destroy(deathAnim, 1f);
            }
            if (RoomSignal != null)
                RoomSignal.Raise();
            GetLoot();
            gameObject.SetActive(false);
        }
    }

    private void GetLoot()
    {
        if (LootTable != null)
        {
            var loot = LootTable.GetLoot();
            if (loot != null)
                Instantiate(loot.gameObject, transform.position, Quaternion.identity);
        }
    }

    private IEnumerator Knockback(Rigidbody2D body, float pushTime)
    {
        if (body != null)
        {
            yield return new WaitForSeconds(pushTime);
            body.velocity = Vector2.zero;
            CurrentState = EnemyState.Idle;
        }
    }
}
