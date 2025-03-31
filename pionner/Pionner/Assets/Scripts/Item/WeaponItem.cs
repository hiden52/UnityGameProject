using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : EquipmentItem, IWeaponItem
{
    public WeaponItem(EquipmentItemData weaponItemData) : base(weaponItemData)
    {

    }
    public override void Use()
    {
        Equip();
    }

    public override void Equip()
    {
        EquipmentManager.Instance.Equip(this);
    }

    public void Attack()
    {

    }
}
