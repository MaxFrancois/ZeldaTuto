using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Loot
{
    public Powerup LootItem;
    public int LootChance;
}

[CreateAssetMenu]
public class LootTable : ScriptableObject
{
    public Loot[] Loots;

    public Powerup GetLoot()
    {
        int cumulativeProbability = 0;
        int currentProbability = Random.Range(0, 100);
        for (int i = 0; i < Loots.Length; i++)
        {
            cumulativeProbability += Loots[i].LootChance;
            if (currentProbability <= cumulativeProbability)
            {
                return Loots[i].LootItem;
            }
        }
        return null;
    }
}
