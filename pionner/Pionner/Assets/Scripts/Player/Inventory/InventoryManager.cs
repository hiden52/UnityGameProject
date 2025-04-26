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
            Debug.LogError("활성화 된 인벤토리 칸 보다 소지한 아이템이 더 많다.");
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
        // 인벤토리에 같은 아이템이 있는지 찾는다
        CountableItem existingItem = Items.Find(item => FindExistItem(item, itemData)) as CountableItem;

        if (existingItem != null)
        {
            int overflow = existingItem.Add(amount);
            if (overflow > 0)
            {
                if(Items.Count >= AvailableSlotCount) 
                {
                    Debug.LogError($"인벤토리 부족으로 {itemData.itemName} 아이템 {overflow} 개 추가 실패");
                    // 오버플로우 수 만큼 되돌리는 로직?
                    // 아니면 필드에 드랍하는 로직?
                    return;
                }
                CreateItem(itemData, overflow);
            }
        }
        else
        {
            if (Items.Count >= AvailableSlotCount)
            {
                Debug.LogError("활성화 된 인벤토리 칸 보다 소지한 아이템이 더 많다.");
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

    // CountableItem 이고 최대스택 이하의 아이템일 경우 true
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
