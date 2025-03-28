using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EquipmentManager : Singleton<EquipmentManager>
{
    private Dictionary<EuipmentType, EquipmentItem> equipments = new Dictionary<EuipmentType, EquipmentItem>();

    public void Equip(EquipmentItem equip)
    {
        EquipmentItemData equipData = equip.data as EquipmentItemData;
        if (equipData == null) return;

        // 여기 작성중
        equipments.TryGetValue(equipData.euipmentType, out EquipmentItem a);
        equipments.TryAdd(equipData.euipmentType, equip);
    }

}
