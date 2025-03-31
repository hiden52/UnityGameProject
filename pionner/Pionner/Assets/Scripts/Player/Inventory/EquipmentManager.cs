using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EquipmentManager : Singleton<EquipmentManager>
{
    private Dictionary<EuipmentType, EquipmentItem> equipDictionary = new Dictionary<EuipmentType, EquipmentItem>();

    public void Equip(EquipmentItem equipItem)
    {

        // 여기 작성중
        equipDictionary.TryGetValue(equipItem.EuipType, out EquipmentItem previousItem);
        if (previousItem == null)
        {
            InventoryManager.Instance.AddItem(previousItem.data, 1);
        }

        equipDictionary.TryAdd(equipItem.EuipType, equipItem);
    }

}
