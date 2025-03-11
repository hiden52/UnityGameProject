using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EffectType
{
    Heal,

}
public class ConsumableItemData : CountableItemData
{
    EffectType effectType;

    private void OnEnable()
    {
        itemType = ItemType.Consumable;
    }
}
