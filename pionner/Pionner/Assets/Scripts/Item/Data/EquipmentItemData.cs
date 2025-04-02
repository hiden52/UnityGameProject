using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentWhere
{
    Head,
    Body,
    Hand,
    Foot,
}

public abstract class EquipmentItemData : ItemData
{
    public EquipmentWhere euipmentType;

    
}
