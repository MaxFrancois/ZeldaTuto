using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthTotemCircle : MonoBehaviour
{
    EarthTotemConfig config;
    void Initialize(EarthTotemConfig cfg)
    {
        config = cfg;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.isTrigger && collision.CompareTag("Player"))
        {
            var script = collision.GetComponent<PlayerMovement>();
        }
    }
}
