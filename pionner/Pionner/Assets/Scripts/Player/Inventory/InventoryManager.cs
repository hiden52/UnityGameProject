using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

// 05-02 :: ToDo - HandleInventorySlotUpdate �޼��� ����, �̱��� ����
public class InventoryManager : Singleton<InventoryManager>, IInventoryActions //,ISlotUIController
{
    [SerializeField] private InventoryData inventoryData;
    private int DEFAULT_SLOT_COUNT = 20;
    public int MaxSlotCount => inventoryData.MaxSlotCount;
    public int AvailableSlotCount => inventoryData.AvailableSlotCount;
    public List<Item> Items => inventoryData.items;

    // 2025-04-30   ���� ���յ� �ذ��ؾ���
    //  --> �̱��� ����, (���񽺷������� ���� ���� - (05.02)�Ƚᵵ �� ��?)
    public static event Action<List<Item>> OnInventoryChanged;  // Items ��ü��ȸ - �����϶�
    public event Action<int, Item> OnSlotUpdated;               // ������� �ִ� �����۸� ������Ʈ 
    public event Action<Item> OnItemRemoved;

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
                //OnSlotUpdated?.Invoke(emptySlotIndex, newItem);
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
    // �� �ڵ带 �Ʒ� �ڵ�� �ٲ� ����.
    private List<int> FindAllStackableItemIndexes(CountableItemData itemData)
    {
        List<int> result = new List<int>();
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i] is CountableItem countable && countable.Data == itemData && countable.currentStack < itemData.maxStack)
            {
                result.Add(i);
            }
        }
        return result;
    }
    
    private void AddCoutableItem(CountableItemData itemData, int amount)
    {
        int amountToAdd = amount;
        List<int> stackbleIndexes = FindAllStackableItemIndexes(itemData);
        List<CountableItem> existingStacks = FindAllStackableItems(itemData);

        foreach (int i in stackbleIndexes)
        {
            CountableItem countableItem = Items[i] as CountableItem;
            int canAdd = itemData.maxStack - countableItem.currentStack;
            int addedAmount = Math.Min(amountToAdd, canAdd);
            countableItem.Add(addedAmount);
            amountToAdd -= addedAmount;

            if(amountToAdd <= 0)
            {
                OnSlotUpdated(i, countableItem);
                return;
            }
        }
        /*
        foreach (CountableItem item in existingStacks)
        {
            int canAdd = itemData.maxStack - item.currentStack;
            int added = Mathf.Min(amountToAdd, canAdd);
            item.Add(added);
            amountToAdd -= added;

            if (amountToAdd <= 0)
            {
                OnInventoryChanged?.Invoke(Items);
                // 05.02 :: FindAllStackableItemIndexes ����ؼ� �Ʒ� �̺�Ʈ ����� ����
                //OnSlotUpdated?.Invoke()

                return;
            }
        }
        */

        // new CountableItem stack
        while (amountToAdd > 0)
        {
            if(GetCurrentItemCount() >= AvailableSlotCount)
            {
                Debug.LogError($"[InventoryManager] Fail to Add \"{itemData.itemName}\" - ({amountToAdd}) Inventory is Full. ( {GetCurrentItemCount()} / {AvailableSlotCount} )");
                
                // ���� ������ ó�� ����
                //

                // ���� �̹� �߰�->���԰��� �̺�Ʈ Ʈ���� �� ���ƿ��� ������ ���̻� ������ �ʿ䰡 ����.
                //OnInventoryChanged?.Invoke(Items); // �κ������� �߰��Ǿ��� �� �����Ƿ� �˸�
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
                    OnSlotUpdated(emptySlotIndex, newItem);
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

    public void RemoveItemByItem(Item removeItem)
    {
        int idx = Items.IndexOf(removeItem);
        if (removeItem == null || idx == -1)
        {
            return;
        }
        Items[idx] = null;
        OnSlotUpdated?.Invoke(idx, null);
        OnItemRemoved?.Invoke(removeItem);
    }


    public void RemoveItemAtIndex(int index)
    {
        if(!IsIndexValid(index))
        {
            return;
        }

        if (Items[index] != null)
        {
            Item removeItem = Items[index];
            inventoryData.items[index] = null;
            OnSlotUpdated?.Invoke(index, null);
            OnItemRemoved?.Invoke(removeItem);
        }
    }

    // ���� �޼���� ��ü����.
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

    public Item GetItem(int index)
    {
        if(IsIndexValid(index))
        {
            return Items[index];
        }
        else
        {
            return null;
        }
    }

    public bool ContainsItem(Item item)
    {
        if (item == null)
        {
            return false;
        }

        return Items.Contains(item);
    }
    public List<Item> GetAllItems()
    {
        return Items;
    }
    private bool IsIndexValid(int index)
    {
        return index >= 0 && index < Items.Count;
    }
}
