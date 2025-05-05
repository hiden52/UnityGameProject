using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Progress;

public class QuickSlotManager : MonoBehaviour, IQuickSlotActions
{
    // 2025-05-01 리팩터, 드래그인 드랍 해결해야함, 상속한 인터페이스도 재고하라. (네이밍, 역할)

    [SerializeField] private MonoBehaviour InventoryManagerMB;
    private IInventoryActions inventoryActions;
    private Item[] items;
    public Item[] Items => items;
    public readonly int MAX_SLOT_COUNT = 10;

    public event Action<int, Item> OnQuickSlotChanged;

    public int GetMaxCount()
    {
        return MAX_SLOT_COUNT;
    }
    public Item[] GetItems()
    {
        return items;
    }
    private void Awake()
    {
        InitializeQuickSlotList();
        if (InventoryManagerMB != null)
        {
            inventoryActions = InventoryManagerMB.GetComponent<IInventoryActions>();
        }

        if(inventoryActions == null)
        {
            Debug.LogError("[QuickSlotManager] Inventory 참조 실패");
        }
    }

    private void OnEnable()
    {
        inventoryActions.OnSlotUpdated += HandleInventorySLotUpdate;
    }
    private void OnDisable()
    {
        inventoryActions.OnSlotUpdated -= HandleInventorySLotUpdate;
    }


    private void HandleInventorySLotUpdate(int index, Item updatedItem)
    {
        for(int i = 0; i< items.Length; i++)
        {
            Item currentItem = items[i];

            if(currentItem != null)
            {
                if(!inventoryActions.ContainsItem(currentItem))
                {
                    items[i] = null;
                    OnQuickSlotChanged?.Invoke(i, null);
                }
                else if(currentItem == updatedItem)
                {
                    OnQuickSlotChanged?.Invoke(i, currentItem);
                }
            }
        }
    }
    private void InitializeQuickSlotList()
    {
        items = new Item[MAX_SLOT_COUNT];
        for (int i = 0; i < MAX_SLOT_COUNT; i++)
        {
            items[i] = null;
        }
    }
    public int FindQuickSlotIndex(Item item)
    {
        if (item == null) return -1;
        return Array.FindIndex(items, i => i == item);
    }

    public void AssignItem(int index, Item item)
    {
        if (!IsIndexValid(index)) return;

        if (item != null && !inventoryActions.ContainsItem(item))
        {
            Debug.LogError("[QuickSlotManager] Not Exist Item in Inventory");
        }

        int existIndex = FindQuickSlotIndex(item);
        if (item != null && existIndex != -1 && index != existIndex)
        {
            Debug.Log($"[QuickSlotManager.AssignItem] existIndex : {existIndex}");
            items[existIndex] = null;
            OnQuickSlotChanged?.Invoke(existIndex, null);
        }
        Debug.Log($"[QuickSlotManager.AssignItem] index : {index}");
        items[index] = item;
        OnQuickSlotChanged?.Invoke(index, item);
    }

    public Item GetItem(int index)
    {
        if (!IsIndexValid(index))
        {
            return items[index];
        }
        else
        {
            Debug.LogError("[QuickSlotManager] Invalid index!");
            return null;
        }
    }

    public void UnassignItem(int index)
    {
        items[index] = null;
        OnQuickSlotChanged?.Invoke(index, null);
    }

    public void MoveOrSwapAssignment(int sourceIndex, int targetIndex)
    {
        if(!IsIndexValid(sourceIndex) || !IsIndexValid(targetIndex) || sourceIndex == targetIndex)
        {
            return;
        }

        Item sourceItme = items[sourceIndex];
        Item targetItme = items[targetIndex];

        AssignItem(targetIndex, sourceItme);
        AssignItem(sourceIndex, targetItme);
    }

    private bool IsIndexValid(int index)
    {
        return index >= 0 && index < MAX_SLOT_COUNT;
    }

}
