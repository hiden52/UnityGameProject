using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingIconSlotUI : SlotUI
{
    private Text BuildingName;

    protected override void Awake()
    {
        base.Awake();

        BuildingName = text.GetComponent<Text>();
    }

    public void SetSlot(Sprite icon, string name)
    {
        Debug.Log(name);
        BuildingName.text = name;
        this.icon.sprite = icon;
        text.SetActive(true);
    }    
}
