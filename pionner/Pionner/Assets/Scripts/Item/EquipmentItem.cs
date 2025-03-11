using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentItem : Item
{
    public EquipmentItem(EquipmentItemData data) : base(data)
    {  

    }
    public override void Use()
    {
        Equip();
    }

    public void Equip()
    {

    }
}
