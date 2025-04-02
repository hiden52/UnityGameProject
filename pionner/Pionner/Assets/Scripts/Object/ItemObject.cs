using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemObject : DefaultObject, IInteratable
{
    [SerializeField] protected ItemData itemData;
    public virtual void Interact()
    {
        InventoryManager.Instance.AddItem(itemData, 1);
    }
}
