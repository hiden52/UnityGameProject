using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceItemData : CountableItemData
{
    private void OnEnable()
    {
        itemType = ItemType.Resource;
    }
}
