using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class QuickSlotManager : MonoBehaviour, IQuickSlotActions
{
    // 2025-05-01 리팩터, 드래그인 드랍 해결해야함, 상속한 인터페이스도 재고하라. (네이밍, 역할)

    [SerializeField] private IInventoryActions inventoryManager;
    private Item[] items;
    public Item[] Items => items;
    public readonly int MAX_SLOT_COUNT = 10;

    public event Action<int, Item> OnQuickSlotChanged;

    private void Awake()
    {
        InitializeQuickSlotList();
        if(inventoryManager == null)
        {
            Debug.LogError("[QuickSlotManager] Inventory 참조 실패");
        }
    }

    private void OnEnable()
    {
        inventoryManager.OnSlotUpdated += HandleInventorySLotUpdate;
    }
    private void OnDisable()
    {
        inventoryManager.OnSlotUpdated -= HandleInventorySLotUpdate;
    }


    private void HandleInventorySLotUpdate(int index, Item updatedItem)
    {
        for(int i = 0; i< items.Length; i++)
        {
            Item currentItem = items[i];

            if(currentItem != null)
            {
                if(!inventoryManager.ContainsItem(currentItem))
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

    public void AssignItem(int index, Item item)
    {
        if (!IsIndexValid(index)) return;
        items[index] = item;
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
    }

    public void MoveOrSwapAssignment(int sourceIndex, int targetIndex)
    {

    }

    private bool IsIndexValid(int index)
    {
        return index >= 0 && index < MAX_SLOT_COUNT;
    }

}
