using System.Collections;
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

    private void Awake()
    {
        CurrentHealth = MaxHealth.InitialValue;
    }

    public void Knock(Rigidbody2D body, float pushTime, float damage)
    {
        StartCoroutine(Knockback(body, pushTime));
        UpdateHealth(damage);
    }

    private void UpdateHealth(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            gameObject.SetActive(false);
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
