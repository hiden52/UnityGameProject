using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : EquipmentItem, IWeaponItem
{
    public WeaponItem(EquipmentItemData weaponItemData) : base(weaponItemData)
    {
        data = weaponItemData;
    }

    public float GetDamage()
    {
        if (Data is WeaponItemData weaponData)
        {
            return weaponData.Damage;
        }
        return 1f; // �⺻ ������
    }

    public WeaponType GetWeaponType()
    {
        if (Data is WeaponItemData weaponData)
        {
            return weaponData.WeaponType;
        }
        return WeaponType.None; 
    }

    public void Attack()
    {
        // Sound effect
    }
}
