using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public Sprite ItemSprite;
    public string ItemName;
    public string ItemDescription;
    public bool isKey;
}
