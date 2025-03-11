using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountableItem : Item
{
    public int currentStack { get; private set; }

    public CountableItem(CountableItemData data) : base(data)
    {

    }
    
    public override void Use()
    {
        Debug.Log(data.itemName);
    }

}
