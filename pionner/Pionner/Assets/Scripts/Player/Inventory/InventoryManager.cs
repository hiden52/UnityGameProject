using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [SerializeField] private InventoryData inventoryData;
    private int DEFAULT_SLOT_COUNT = 20;
    public int MaxSlotCount => inventoryData.MaxSlotCount;
    public int AvailableSlotCount => inventoryData.AvailableSlotCount;
    public List<Item> Items => inventoryData.items;

    public static event Action<List<Item>> OnInventoryChanged;

    [Header("Debug")]
    [SerializeField]
    private bool debugPrintItems = false;

    protected override void Awake()
    {
        base.Awake();
        inventoryData.SetCurrentSlotCount(DEFAULT_SLOT_COUNT);
    }

    private void Update()
    {
        if(debugPrintItems)
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
    }
    private void AddSingleItem(ItemData itemData)
    {
        if (Items.Count >= AvailableSlotCount)
        {
            Debug.LogError("Ȱ��ȭ �� �κ��丮 ĭ ���� ������ �������� �� ����.");
            return;
        }

        Item newItem = ItemFactory.CreateItem(itemData, 1);
        if (newItem != null)
        {
            AddToSlot(newItem);
        }
    }

    private void AddCoutableItem(CountableItemData itemData, int amount)
    {
        // �κ��丮�� ���� �������� �ִ��� ã�´�
        CountableItem existingItem = Items.Find(item => FindExistItem(item, itemData)) as CountableItem;

        if (existingItem != null)
        {
            int overflow = existingItem.Add(amount);
            if (overflow > 0)
            {
                if(Items.Count >= AvailableSlotCount) 
                {
                    Debug.LogError($"�κ��丮 �������� {itemData.itemName} ������ {overflow} �� �߰� ����");
                    // �����÷ο� �� ��ŭ �ǵ����� ����?
                    // �ƴϸ� �ʵ忡 ����ϴ� ����?
                    return;
                }
                CreateItem(itemData, overflow);
            }
        }
        else
        {
            if (Items.Count >= AvailableSlotCount)
            {
                Debug.LogError("Ȱ��ȭ �� �κ��丮 ĭ ���� ������ �������� �� ����.");
                return;
            }
            CreateItem(itemData, amount);
        }
        
    }
    public void AddItem(ItemData itemData, int amount = 1)
    {
        if(itemData is CountableItemData)
        {
            AddCoutableItem(itemData as CountableItemData, amount);
        }
        else
        {
            AddSingleItem(itemData);
        }

        OnInventoryChanged?.Invoke(Items);
    }

    private void CreateItem(ItemData itemData, int amount)
    {
        Item newItem = ItemFactory.CreateItem(itemData, amount);
        if (newItem != null)
        {
            AddToSlot(newItem);
        }
    }
    private void AddToSlot(Item item)
    {
        int emptySlotIndex = Items.FindIndex(i => i == null);

        if(emptySlotIndex != -1)
        {
            Items[emptySlotIndex] = item;
        }
        else
        {
            Items.Add(item);

        }
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
        OnInventoryChanged?.Invoke(Items);
    }

    // CountableItem �̰� �ִ뽺�� ������ �������� ��� true
    bool FindExistItem(Item item, ItemData itemData)
    {
        if (item is CountableItem countable && countable.Data == itemData)
        {
            var countableData = countable.Data as CountableItemData;

            return countable.currentStack < countableData.maxStack;
        }
        else
        {
            return false;
        }
        
    }

}
