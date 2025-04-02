using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Tool,
    Gun,
    RemoteSwitch,
    Sword,
}

[CreateAssetMenu(fileName = "NewWeaponItemData", menuName = "Items/Equipment Item Data/Weapon Item Data")]
public class WeaponItemData : EquipmentItemData
{
    public WeaponType WeaponType;
    private void OnEnable()
    {
        itemType = ItemType.Weapon;
        euipmentType = EquipmentWhere.Hand;
    }
}
