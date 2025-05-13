using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Resource = 0b0010000,
    Equipment = 0b0100000,
    Consumable = 0b1000000,
    Weapon = 0b0100001,
    Armor = 0b0100010,
}

public abstract class ItemData : ScriptableObject
{
    public int itemID;
    public string itemName;
    [TextArea] public string Description;
    public Sprite icon;
    public ItemType itemType;
    public GameObject prefab;

}
