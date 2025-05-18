using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FnClearInventory : Fn
{

    

    protected override void Awake()
    {
        base.Awake();
        str = "Clear\nInventory";
    }


    protected override void OnClick()
    {
        Debug.Log("Clear Inventory");
        InventoryManager.Instance.ClearInventory();
    }

}
