using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [SerializeField] private InventoryData inventoryData;
    private int DEFAULT_SLOT_COUNT = 20;
    public int MaxSlotCount => inventoryData.MaxSlotCount;
    public int CurrentSlotCount => inventoryData.currentSlotCount;
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

    public void AddItem(ItemData itemData, int amount = 1)
    {
        if(Items.Count > CurrentSlotCount)
        {
            Debug.LogError("Ȱ��ȭ �� �κ��丮 ĭ ���� ������ �������� �� ����.");
            return;
        }    

        if (itemData is EquipmentItemData)
        {
            Item newItem = ItemFactory.CreateItem(itemData, amount);
            if (newItem != null)
            {
                Items.Add(newItem);
            }
        }
        else
        {
            // �κ��丮�� CountableItem�� itemData�� �����ϴ��� Ȯ��
            CountableItem existingItem = Items.Find(item => FindExistItem(item, itemData)) as CountableItem;


            // �κ��丮 ĭ�� Ȯ���ϰ�, ĭ�� ��� á�ٸ� �޼����� ���� AddItem�� �������� �ʵ���
            if (existingItem != null)
            {
                int overflow = existingItem.Add(amount);
                if (overflow > 0)
                {
                    CountableItem newItem = ItemFactory.CreateItem(itemData, amount) as CountableItem;
                    Items.Add(newItem);
                }
            }
            else
            {
                Item newItem = ItemFactory.CreateItem(itemData, amount);
                if (newItem != null)
                {
                    Items.Add(newItem);
                }
            }
        }

        OnInventoryChanged?.Invoke(Items);
    }

    public void RemoveItem(Item item)
    {
        if (Items.Contains(item))
        {
            Items.Remove(item);
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
        if (item is CountableItem countable && countable.data == itemData)
        {
            var countableData = countable.data as CountableItemData;

            return countable.currentStack < countableData.maxStack;
        }
        else
        {
            return false;
        }
        
    }

}
