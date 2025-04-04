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
    [SerializeField] private float damage;

    public float Damage => damage;
    private void OnEnable()
    {
        itemType = ItemType.Weapon;
        euipmentType = EquipmentWhere.Hand;
    }
}
