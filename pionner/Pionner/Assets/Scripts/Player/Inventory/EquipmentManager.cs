using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class EquipmentManager : Singleton<EquipmentManager>
{
    [System.Serializable]
    private struct EquipmentSlot
    {
        public EquipmentWhere equipWhere;
        public Transform socket;
    }

    [SerializeField] private EquipmentSlot[] equipmentSlots;

    private Dictionary<EquipmentWhere, EquipmentItem> equipDictionary;
    private Dictionary<EquipmentWhere, Transform> equipSockets;
    private Dictionary<EquipmentWhere, GameObject> currentEquipObjects;

    public Item ItemOnHand => equipDictionary.TryGetValue(EquipmentWhere.RightHand, out EquipmentItem item) ? item : null;

    public static event Action OnEquimentChagned;
    public event Action<EquipmentWhere, EquipmentItem> OnSlotUpdated;
    protected override void Awake()
    {
        base.Awake();
        equipSockets = new Dictionary<EquipmentWhere, Transform>();
        equipDictionary = new Dictionary<EquipmentWhere, EquipmentItem>();
        currentEquipObjects = new Dictionary<EquipmentWhere, GameObject>();

        foreach (var slot in equipmentSlots)
        {
            if(!equipSockets.ContainsKey(slot.equipWhere))
            {
                equipSockets.Add(slot.equipWhere, slot.socket);
            }
        }

    }

    public void Equip(EquipmentItem equipItem)
    {
        if(equipDictionary.ContainsKey(equipItem.EuipType))
        {
            Unequip(equipItem.EuipType);
        }

        equipDictionary.Add(equipItem.EuipType, equipItem);

        InstantiateEquipment(equipItem);
        OnEquimentChagned?.Invoke();
        OnSlotUpdated?.Invoke(equipItem.EuipType, equipItem);
    }

    public void Unequip(EquipmentWhere equipWhere )
    {
        if (equipDictionary.TryGetValue(equipWhere, out EquipmentItem currentItem))
        {
            InventoryManager.Instance.AddItem(currentItem.Data);
            equipDictionary.Remove(equipWhere);

            if(currentEquipObjects.TryGetValue(equipWhere, out GameObject equipObject))
            {
                ObjectPool.Instance.ReturnObject(equipObject);
                currentEquipObjects.Remove(equipWhere);
            }
            else
            {
                Debug.LogError($"[EquipmentManager] {currentItem.Data.itemName} instance not found in currentEquipObjects.");
            }
        }
        OnEquimentChagned?.Invoke();
        OnSlotUpdated?.Invoke(equipWhere, null);
    }
    


    

    public void InstantiateEquipment(EquipmentItem equipItem)
    {
        GameObject newEquipment = ObjectPool.Instance.GetObject(equipItem.Data.prefab);
        if (equipSockets.TryGetValue(equipItem.EuipType, out Transform socket))
        {
            newEquipment.transform.SetParent(socket);
            newEquipment.transform.localPosition = Vector3.zero;
            newEquipment.transform.localRotation = Quaternion.identity;

            currentEquipObjects.Add(equipItem.EuipType, newEquipment);
        }
        else
        {
            Debug.LogWarning("[EquipmentManager] No socket mapped for EquipmentWhere : " + equipItem.EuipType);
            ObjectPool.Instance.ReturnObject(newEquipment);
        }
    }
}
