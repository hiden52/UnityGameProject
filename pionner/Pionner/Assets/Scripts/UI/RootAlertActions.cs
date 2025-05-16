using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RootAlertActions : AlertActions
{
    [SerializeField] Image itemIconImage;
     
    protected override void Awake()
    {
        base.Awake();

        if(itemIconImage == null)
        {
            itemIconImage = GetComponentInChildren<Image>();
        }
    }

    public void SetAlert(ItemData itemData)
    {
        alertText.text = itemData.itemName;
        itemIconImage.sprite = itemData.icon;
    }

    
}
