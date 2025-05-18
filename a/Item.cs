using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public abstract class Item
{
    protected ItemData data;
    public ItemData Data => data;

    public Item(ItemData data)
    {
        this.data = data;

    }

    public abstract void Use();
}
