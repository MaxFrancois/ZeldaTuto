using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject
{
    public Item currentItem;
    public List<Item> Items = new List<Item>();
    public int NumberOfKeys;

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
