using System.Collections.Generic;
using System;
using UnityEngine;


public interface IInventoryActions
{
  
    Item GetItem(int index);
    void MoveOrSwapItem(int sourceIndex, int targetIndex); 
    void RemoveItemAtIndex(int index);
    bool ContainsItem(Item item);
    List<Item> GetAllItems(); 

    event Action<int, Item> OnSlotUpdated;
    event Action<Item> OnItemRemoved;
}