using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountableItem : Item
{
    public int currentStack { get; private set; }

    public CountableItem(CountableItemData data) : base(data)
    {
        currentStack = 1;
    }
    
    public override void Use()
    {
        Debug.Log(data.itemName + " was used.");
        if(currentStack > 0)
        {
            currentStack--;
        }
        else
        {
            InventoryManager.Instance.RemoveItemByItem(this);
        }
    }
    public void Reduce(int amount)
    {
        int amoutToReduce = amount;
        if (currentStack > 0)
        {
            currentStack -= amount;
        }
    }
    // CountableItemData.maxStack의 초과 값을 반환한다.
    public int Add(int amount)
    {
        CountableItemData countableItemData = data as CountableItemData;
        int totalAmount = currentStack + amount;
        
        if(totalAmount <= countableItemData.maxStack)
        {
            currentStack = totalAmount;

            return 0;
        }
        else
        {
            int overflow = totalAmount - countableItemData.maxStack;
            currentStack = countableItemData.maxStack;

            return overflow;
        }
    }

}
