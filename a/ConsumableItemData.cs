using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EffectType
{
    Heal,

}

[CreateAssetMenu(fileName = "New Consumable Item Data", menuName = "Items/Consumable Item Data")]
public class ConsumableItemData : CountableItemData
{
    EffectType effectType;

    private void OnEnable()
    {
        itemType = ItemType.Consumable;
    }
}
