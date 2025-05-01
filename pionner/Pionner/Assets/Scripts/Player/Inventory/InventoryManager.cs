using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 05-01 :: IInventoryActions �������̽� Ȯ�� �� ����, HandleInventorySlotUpdate �޼��� ����
public class InventoryManager : Singleton<InventoryManager>, ISlotUIController
{
    [SerializeField] private InventoryData inventoryData;
    private int DEFAULT_SLOT_COUNT = 20;
    public int MaxSlotCount => inventoryData.MaxSlotCount;
    public int AvailableSlotCount => inventoryData.AvailableSlotCount;
    public List<Item> Items => inventoryData.items;

    // 2025-04-30 ���� ���յ� �ذ��ؾ���.
    public static event Action<List<Item>> OnInventoryChanged;

    [Header("Debug")]
    [SerializeField]
    private bool debugPrintItems = false;

    protected override void Awake()
    {
        base.Awake();
        inventoryData.SetCurrentSlotCount(DEFAULT_SLOT_COUNT);
        InitializeInvetoryList();
    }

    private void InitializeInvetoryList()
    {
        List<Item> currentItems = Items;
        inventoryData.items = new List<Item>(AvailableSlotCount);
        for (int i = 0; i < AvailableSlotCount; i++)
        {
            if (i < currentItems.Count && currentItems[i] != null)
            {
                Items.Add(currentItems[i]);
            }
            else
            {
                Items.Add(null); // �� ����
            }
        }
    }

    private int GetCurrentItemCount()
    {
        int count = 0;
        foreach (Item item in Items)
        {
            if(item != null)
            {
                count++;
            }
        }
        return count;
    }

    private void Update()
    {
        // Debug
        /*
        if (debugPrintItems)
        {
            if (Items.Count > 0)
            {
                foreach (var item in Items)
                {
                    if (item is CountableItem countableItem)
                    {
                        //Debug.Log($"{countableItem.data.itemName} ({countableItem.currentStack})");
                    }
                    else
                    {
                        //Debug.Log(item.data.itemName);
                    }
                }
            }
            else
            {
                Debug.Log("Invetory is Empty!");
            }

            debugPrintItems = false;
        }
        */
    }
    private void AddSingleItem(ItemData itemData)
    {
        if (GetCurrentItemCount() >= AvailableSlotCount)
        {
            Debug.LogError($"�κ��丮 ���� �������� {itemData.itemName} ������ �߰� ����");
            
            // �߰����� ���� ������ ó�� ����
            // ���?

            return;
        }

        // �� ���� ã��
        int emptySlotIndex = Items.FindIndex(i => i == null);
        if (emptySlotIndex != -1)
        {
            Item newItem = ItemFactory.CreateItem(itemData, 1);
            if (newItem != null)
            {
                Items[emptySlotIndex] = newItem;
                OnInventoryChanged?.Invoke(Items); 
                Debug.Log($"{itemData.itemName} 1���� Slot_{emptySlotIndex}�� �߰�.");
            }
            else
            {
                Debug.LogError($"{itemData.itemName} ������ ���� ����!");
            }
        }
        else
        {
            Debug.LogError($"�κ��丮�� ����({AvailableSlotCount - GetCurrentItemCount()}��)�� ������ �� ����(null)�� ã�� ���߽��ϴ�!");
        }
    }
    private List<CountableItem> FindAllStackableItems(CountableItemData itemData)
    {
        List<CountableItem> result = new List<CountableItem>();
        foreach (Item item in Items)
        {
            if (item is CountableItem countable && countable.Data == itemData && countable.currentStack < itemData.maxStack)
            {
                result.Add(countable);
            }
        }
        return result;
    }
    private void AddCoutableItem(CountableItemData itemData, int amount)
    {
        int amountToAdd = amount;

        List<CountableItem> existingStacks = FindAllStackableItems(itemData);

        foreach (CountableItem item in existingStacks)
        {
            int canAdd = itemData.maxStack - item.currentStack;
            int added = Mathf.Min(amountToAdd, canAdd);
            item.Add(added);
            amountToAdd -= added;

            if (amountToAdd <= 0)
            {
                OnInventoryChanged?.Invoke(Items);
                return;
            }
        }

        // new CountableItem stack
        while (amountToAdd > 0)
        {
            if(GetCurrentItemCount() >= AvailableSlotCount)
            {
                Debug.LogError($"[InventoryManager] Fail to Add \"{itemData.itemName}\" - ({amountToAdd}) Inventory is Full. ( {GetCurrentItemCount()} / {AvailableSlotCount} )");
                
                // ���� ������ ó�� ����
                //

                OnInventoryChanged?.Invoke(Items); // �κ������� �߰��Ǿ��� �� �����Ƿ� �˸�
                return;
            }

            // �� ���� ã��
            int emptySlotIndex = Items.FindIndex(item => item == null);
            if (emptySlotIndex != -1)
            {
                int amountForNewStack = Mathf.Min(amountToAdd, itemData.maxStack);
                Item newItem = ItemFactory.CreateItem(itemData, amountForNewStack);
                if (newItem != null)
                {
                    Items[emptySlotIndex] = newItem;
                    amountToAdd -= amountForNewStack;
                    //Debug.Log($"[InventoryManager] Add {itemData.itemName} ({amountForNewStack}) to Slot_{emptySlotIndex}");
                }
                else
                {
                    Debug.LogError($"[InventoryManager] Fail to create item {itemData.itemName} ");
                    break;
                }
            }
            else
            {
                Debug.LogError($"[InventoryManager] �κ��丮�� ����({AvailableSlotCount - GetCurrentItemCount()}��)�� ������ �� ����(null)�� ã�� ���߽��ϴ�! ����Ʈ ���� Ȯ�� �ʿ�.");
                break; 
            }

            OnInventoryChanged?.Invoke(Items);
        }
    }

    public void MoveOrSwapItem(int sourceIndex, int targetIndex)
    {
        int maxIndex = AvailableSlotCount;
        if (sourceIndex < 0 || sourceIndex >= maxIndex || targetIndex < 0 || targetIndex >= maxIndex)
        {
            Debug.LogError($"[InventoryManager] Item move/swap, Index error : Source = {sourceIndex}, Target = {targetIndex}");
            return;
        }

        while (Items.Count < AvailableSlotCount)
        {
            Items.Add(null);
        }

        //Debug.Log($"Try Item Move/Swap : {sourceIndex} <-> {targetIndex}");

        Item sourceItem = Items[sourceIndex];
        Item targetItem = Items[targetIndex];

        Items[targetIndex] = sourceItem;
        Items[sourceIndex] = targetItem;

        OnInventoryChanged?.Invoke(Items);
    }
    public void AddItem(ItemData itemData, int amount = 1)
    {
        if(itemData is CountableItemData countableItemData)
        {
            AddCoutableItem(countableItemData, amount);
        }
        else
        {
            AddSingleItem(itemData);
        }

        OnInventoryChanged?.Invoke(Items);
    }


    public void RemoveItem(Item item)
    {
        if (Items.Contains(item))
        {
            Items[Items.IndexOf(item)] = null;
            OnInventoryChanged?.Invoke(Items);
        }
    }
    public void ReduceAmount(CountableItem item, int amount)
    {
        if (item == null) return;
    }

    public void ClearInventory()
    {
        Items.Clear();
        while(Items.Count < AvailableSlotCount)
        {
            Items.Add(null);
        }
        OnInventoryChanged?.Invoke(Items);
    }
}
