using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountableItem : Item
{
    private bool isConsumable;
    public int currentStack { get; private set; }

    public CountableItem(CountableItemData data) : base(data)
    {
        isConsumable = false;
        currentStack = 1;
    }
    
    public override void Use()
    {
        if (!isConsumable) return;

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
        if (currentStack >= amount)
        {
            currentStack -= amount;
        }
        else
        {
            Debug.LogWarning($"{this.data.itemName} is {currentStack}. Can't Reduce {amount}!");
        }
    }
    /// <summary>
    /// currentStack에 더하고 초과값을 반환한다.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>CountableItemData.maxStack 의 초과 값을 반환한다.</returns>
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
