using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Resource = 0b0001,
    Equipment = 0b0010,
    Consumable = 0b0100,
}

public abstract class ItemData : ScriptableObject
{
    public int itemID;
    public string itemName;
    [TextArea] public string Description;
    public Sprite icon;
    public ItemType itemType;

}
