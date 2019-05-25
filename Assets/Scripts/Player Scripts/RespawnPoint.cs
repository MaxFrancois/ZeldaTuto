﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public Vector2 RespawnPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.isTrigger)
        {
            collision.GetComponent<PlayerMovement>().SetRespawnPoint(this);
        }
    }
}