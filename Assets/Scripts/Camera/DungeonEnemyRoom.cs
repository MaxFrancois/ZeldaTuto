using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEnemyRoom : DungeonRoom
{
    public Door[] Doors;

    public override void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
        {
            foreach (var enemy in Enemies)
            {
                ChangeActivation(enemy, true);
            }

            foreach (var pot in Pots)
            {
                ChangeActivation(pot, true);
            }
            CloseDoors();
            VirtualCamera.gameObject.SetActive(true);
        }
    }
    
    public void CheckEnemies()
    {
        for (int i = 0; i < Enemies.Length; i++)
            if (Enemies[i].gameObject.activeInHierarchy && i < Enemies.Length -1 )
                return;
        OpenDoors();
    }

    public void CloseDoors()
    {
        foreach (var door in Doors)
        {
            door.Close();
        }
    }

    public void OpenDoors()
    {
        foreach (var door in Doors)
        {
            door.Open();
        }
    }
}
