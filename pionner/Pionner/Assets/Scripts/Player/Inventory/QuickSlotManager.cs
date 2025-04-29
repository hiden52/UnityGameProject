using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotManager : MonoBehaviour
{
    private List<Item> items = new List<Item>();
    public readonly int MAX_SLOT_COUNT = 10;
    public List<Item> Items => items;

    private void Awake()
    {
        InitializeQuickSlotList();
    }


    private void InitializeQuickSlotList()
    {
        if(items.Count <= 0)
        {
            for (int i = 0; i < MAX_SLOT_COUNT; i++)
            {
                items.Add(null);
            }
        }
    }

    private int GetCurrentItemCount()
    {
        int count = 0;
        foreach (Item item in items)
        {
            if (item != null)
            {
                count++;
            }
        }
        return count;
    }

}
