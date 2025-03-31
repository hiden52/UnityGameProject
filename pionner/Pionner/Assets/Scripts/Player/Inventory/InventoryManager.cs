using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    private List<Item> items = new List<Item>();
    public List<Item> Items { get { return items; } }

    public static event Action<List<Item>> OnInventoryChanged;

    [Header("Debug")]
    [SerializeField]
    private bool debugPrintItems = false;

    private void Start()
    {

    }

    private void Update()
    {
        if(debugPrintItems)
        {
            if (items.Count > 0)
            {
                foreach (var item in items)
                {
                    if (item is CountableItem countableItem)
                    {
                        Debug.Log($"{countableItem.data.itemName} ({countableItem.currentStack})");
                    }
                    else
                    {
                        Debug.Log(item.data.itemName);
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

    public void AddItem(ItemData itemData, int amount)
    {

        if (itemData is EquipmentItemData)
        {
            Item newItem = ItemFactory.CreateItem(itemData, amount);
            if (newItem != null)
            {
                items.Add(newItem);
            }
        }
        else
        {
            // �κ��丮�� CountableItem�� itemData�� �����ϴ��� Ȯ��
            CountableItem existingItem = items.Find(item => FindExistItem(item, itemData)) as CountableItem;


            // �κ��丮 ĭ�� Ȯ���ϰ�, ĭ�� ��� á�ٸ� �޼����� ���� AddItem�� �������� �ʵ���
            if (existingItem != null)
            {
                int overflow = existingItem.Add(amount);
                if (overflow > 0)
                {
                    CountableItem newItem = ItemFactory.CreateItem(itemData, amount) as CountableItem;
                    items.Add(newItem);
                }
            }
            else
            {
                Item newItem = ItemFactory.CreateItem(itemData, amount);
                if (newItem != null)
                {
                    items.Add(newItem);
                }
            }
        }

        OnInventoryChanged?.Invoke(items);
    }

    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
        }

        OnInventoryChanged?.Invoke(items);
    }

    // CountableItem �̰� �ִ뽺�� ������ �������� ��� true
    bool FindExistItem(Item item, ItemData itemData)
    {
        if (item is CountableItem countable && countable.data == itemData)
        {
            if (countable.currentStack < ((CountableItemData)countable.data).maxStack)
            {
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
        
    }

}
