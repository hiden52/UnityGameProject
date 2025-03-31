using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentItem : Item
{
    public EquipmentItemData EquipData => data as EquipmentItemData;
    public EuipmentType EuipType => EquipData.euipmentType;

    public EquipmentItem(EquipmentItemData data) : base(data) { }

    public abstract void Equip();
}
