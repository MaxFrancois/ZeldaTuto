using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRoomTrigger : MonoBehaviour
{
    public VoidSignal StartBattleSignal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.isTrigger)
        {
            StartBattleSignal.Raise();
            gameObject.SetActive(false);
        }
    }
}
