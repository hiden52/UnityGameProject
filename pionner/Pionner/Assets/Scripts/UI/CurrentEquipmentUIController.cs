using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CurrentEquipmentUIController : MonoBehaviour
{
    [SerializeField] Image equipmentIcon;
    [SerializeField] Text equipmentName;
    [SerializeField] Item currentItem;
    [SerializeField] Color none = new Color(1, 1, 1, 0);
    [SerializeField] Color iconColor = new Color(1, 1, 1, 1);

    private void OnEnable()
    {
        EquipmentManager.OnEquimentChagned += EquipmentChange;
    }

    private void OnDisable()
    {
        EquipmentManager.OnEquimentChagned -= EquipmentChange;
    }

    private void Start()
    {
        SetNone();
        SetEquipName(string.Empty);
    }

    public void EquipmentChange()
    {
        currentItem = EquipmentManager.Instance.ItemOnHand;

        if(currentItem == null)
        {
            SetNone();
            SetEquipName(string.Empty);
            return;
        }
        
        equipmentIcon.sprite = currentItem.Data.icon;
        equipmentIcon.color = iconColor;
        SetEquipName(currentItem.Data.itemName);
        
    }

    public void SetNone()
    {
        equipmentIcon.sprite = null;
        equipmentIcon.color = none;
    }

    public void SetEquipName(string name)
    {
        equipmentName.text = name;
    }
}
