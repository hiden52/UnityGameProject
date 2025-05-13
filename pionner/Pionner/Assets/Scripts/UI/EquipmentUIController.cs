using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUIController : MonoBehaviour
{
    [SerializeField] private GameObject headSlot;
    [SerializeField] private GameObject handSlot;
    [SerializeField] private GameObject legsSlot;
    [SerializeField] private GameObject bodySlot;
    [SerializeField] private GameObject backSlot;

    Dictionary<EquipmentWhere, ItemSlotUI> equipmentWhereDic = new Dictionary<EquipmentWhere, ItemSlotUI>();

    [SerializeField] EquipmentManager equipmentManager;

    private void Awake()
    {
        equipmentWhereDic.Add(EquipmentWhere.Head, headSlot.GetComponent<ItemSlotUI>());
        equipmentWhereDic.Add(EquipmentWhere.RightHand, handSlot.GetComponent<ItemSlotUI>());
        equipmentWhereDic.Add(EquipmentWhere.Foot, legsSlot.GetComponent<ItemSlotUI>());
        equipmentWhereDic.Add(EquipmentWhere.Body, bodySlot.GetComponent<ItemSlotUI>());
        //equipmentWhereDic.Add(EquipmentWhere.Back, backSlot.GetComponent<SlotUI>());

    }

    private void Start()
    {
        equipmentManager.OnSlotUpdated +=  SetEquipSlot;
    }

    private void OnEnable()
    {
        if(equipmentManager != null)
        {
            equipmentManager.OnSlotUpdated += SetEquipSlot;
        }
    }

    private void OnDisable()
    {
        equipmentManager.OnSlotUpdated -= SetEquipSlot;
    }

    private void SetEquipSlot(EquipmentWhere equipmentWhere, EquipmentItem equipmentItem)
    {
        if(equipmentWhereDic.TryGetValue(equipmentWhere, out var slot))
        {
            slot.SetContainerType(SlotContainerType.EquipmentSlot);
            slot.SetSlot(equipmentItem);
        }
    }

}
