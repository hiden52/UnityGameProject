using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceObject : DefaultObject, IInteratable
{
    [SerializeField] private CountableItemData itemData;



    public void Interact()
    {
        InventoryManager.Instance.AddItem(itemData, 1);
    }
}
