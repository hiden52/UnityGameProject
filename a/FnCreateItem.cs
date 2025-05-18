using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FnCreateItem : Fn
{
    public ResourceItemData target;

    protected override void Awake()
    {
        base.Awake();
        str = "Create\nItem";
    }

    protected override void OnClick()
    {
        if(target == null)
        {
            Debug.LogWarning("Missing target Item Data!");
            return;
        }
        InventoryManager.Instance.AddItem(target, 32);
    }

}
