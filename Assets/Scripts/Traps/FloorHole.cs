using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorHole : MonoBehaviour
{
    [SerializeField] float Damage = default;
    [SerializeField] bool FallTowardsCenter = default;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            if (collision.GetComponent<CharacterState>().MovementState != CharacterMovementState.Dashing)
                collision.GetComponent<PlayerMovement>().TriggerFall(Damage, FallTowardsCenter ? GetComponent<Collider2D>().bounds.center : Vector3.zero);
        }
        if (collision.CompareTag("Enemy") && !collision.isTrigger)
        {
            if (collision.GetComponent<CharacterState>().MovementState != CharacterMovementState.Dashing)
                collision.GetComponent<EnemyBase>().TriggerFall(FallTowardsCenter ? GetComponent<Collider2D>().bounds.center : Vector3.zero);
        }
    }

    public void TriggerFall(GameObject player)
    {
        player.GetComponent<PlayerMovement>().TriggerFall(Damage, FallTowardsCenter ? GetComponent<Collider2D>().bounds.center : Vector3.zero);
    }
}
