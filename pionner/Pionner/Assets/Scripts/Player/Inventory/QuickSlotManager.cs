using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class QuickSlotManager : MonoBehaviour, ISlotUIController
{
    // 2025-04-30 리팩터, 드래그인 드랍 해결해야함, 상속한 인터페이스도 재고하라. (네이밍, 역할)
    private Item[] items;
    public Item[] Items => items;
    public readonly int MAX_SLOT_COUNT = 10;

    public event Action OnQuickSlotChanged;

    private void Awake()
    {
        InitializeQuickSlotList();
    }


    private void InitializeQuickSlotList()
    {
        items = new Item[MAX_SLOT_COUNT];
        for (int i = 0; i < MAX_SLOT_COUNT; i++)
        {
            items[i] = null;
        }
    }

    public void AssignItem(Item item, int index)
    {
        items[index] = item;
    }

    public void UnassignItem(int index)
    {
        items[index] = null;
    }

    public void MoveOrSwapItem(int sourceIndex, int targetIndex)
    {

    }

}
