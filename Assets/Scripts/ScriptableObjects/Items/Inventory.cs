using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Items/Inventory")]
public class Inventory : ScriptableObject
{
    public Item currentItem;
    public List<Item> Items = new List<Item>();
    public int NumberOfKeys;
    public int Coins;
    public float MaxMana = 100f;
    public float CurrentMana;
    public float PassiveManaRegenSpeed;
    public float CurrentUltimate = 0;
    public float MaxUltimate = 100f;
    public float PassiveUltimateRegenSpeed;

    public void AddItem(Item item)
    {
        if (item.isKey) NumberOfKeys++;
        Items.Add(item);
    }

    public void RemoveItem(Item item)
    {
        if (item.isKey) NumberOfKeys--;
        if (Items.Contains(item))
            Items.Remove(item);
    }
}
