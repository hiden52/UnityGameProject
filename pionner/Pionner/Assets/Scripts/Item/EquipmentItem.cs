using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentItem : Item
{
    public EquipmentItemData EquipData => data as EquipmentItemData;
    public EquipmentWhere EuipType => EquipData.euipmentType;

    public EquipmentItem(EquipmentItemData data) : base(data) { }

    protected void Equip()
    {
        EquipmentManager.Instance.Equip(this);
    }
    public override void Use()
    {
        InventoryManager.Instance.RemoveItemByItem(this);
        Equip();
    }
}
