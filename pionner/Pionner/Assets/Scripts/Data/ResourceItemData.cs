using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource Item Data", menuName = "Items/Resource Item Data")]
public class ResourceItemData : CountableItemData
{
    private void OnEnable()
    {
        itemType = ItemType.Resource;
    }
}
