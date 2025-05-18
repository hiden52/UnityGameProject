using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItemData
{
    public int itemID;
    public int amount;

    public InventoryItemData(int id, int count)
    {
        itemID = id;
        amount = count;
    }
}