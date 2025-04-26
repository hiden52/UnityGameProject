using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentWhere
{
    Head,
    Body,
    LeftHand,
    RightHand,
    Foot,
}

public abstract class EquipmentItemData : ItemData
{
    public EquipmentWhere euipmentType;

    
}
