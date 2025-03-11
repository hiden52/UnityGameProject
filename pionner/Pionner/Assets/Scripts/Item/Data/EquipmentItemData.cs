using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EuipmentType
{
    Head,
    Body,
    Hand,
    Foot,
}
[CreateAssetMenu(fileName = "New Equipment Item Data", menuName = "Items/Equipment Item Data")]
public class EquipmentItemData : ItemData
{
    public EuipmentType euipmentType;

    private void OnEnable()
    {
        itemType = ItemType.Equipment;
    }
}
