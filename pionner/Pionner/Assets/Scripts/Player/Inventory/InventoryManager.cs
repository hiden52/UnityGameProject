using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    
    
    private List<Item> items = new List<Item>();
    public List<Item> Items { get { return items; } }

    public static event Action<List<Item>> OnInventoryChanged;

    [Header("Debug")]
    [SerializeField]
    private bool debugPrintItems = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

    }

    private void Update()
    {
        if(debugPrintItems)
        {
            if (items.Count > 0)
            {
                foreach (var item in Items)
                {
                    Debug.Log(item.data.itemName);
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
        CountableItem existingItem = items.Find(item => item.data == itemData) as CountableItem;

        if(existingItem != null)
        {
            int overflow = existingItem.Add(amount);
            if(overflow > 0)
            {
                CountableItem newItem = ItemFactory.CreateItem(itemData, amount) as CountableItem;
                items.Add(newItem);
            }
        }
        else
        {
            Item newItem = ItemFactory.CreateItem(itemData, amount);
            if(newItem != null)
            {
                items.Add(newItem);
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

}
