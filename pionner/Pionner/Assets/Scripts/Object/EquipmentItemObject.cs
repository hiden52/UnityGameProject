using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentItemObject : ItemObject
{
    public override void Interact()
    {
        EquipmentItemData equipData = itemData as EquipmentItemData;
        InventoryManager.Instance.AddItem(equipData);
    }
}
