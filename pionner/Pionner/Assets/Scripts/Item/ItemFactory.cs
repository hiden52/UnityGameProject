using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Factory ∆–≈œ 
public static class ItemFactory
{
    public static Item CreateItem (ItemData itemData, int amount)
    {
        switch(itemData.itemType)
        {
            case ItemType.Consumable:
                ConsumableItemData consumableData  = itemData as ConsumableItemData;
                CountableItem newConsumableItem = new CountableItem(consumableData);
                newConsumableItem.Add(amount - 1);
                return newConsumableItem;

            case ItemType.Resource:
                ResourceItemData resourceData = itemData as ResourceItemData;
                CountableItem newResourceItem = new CountableItem(resourceData);
                newResourceItem.Add(amount - 1);
                return newResourceItem;

            case ItemType.Weapon:
                return new WeaponItem(itemData as EquipmentItemData);

            default:
                Debug.LogWarning("Invalid Item Type");
                return null;
        }
    }
}
