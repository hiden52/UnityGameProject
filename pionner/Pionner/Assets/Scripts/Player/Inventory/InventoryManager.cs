using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    [Header("Item list")]
    [SerializeField] private List<Item> items = new List<Item>();
    public List<Item> Items { get { return items; } }

    public event Action OnInventoryChanged;

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

    public void AddItem(Item item)
    {
        items.Add(item);
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
        }
        OnInventoryChanged?.Invoke();
    }

}
