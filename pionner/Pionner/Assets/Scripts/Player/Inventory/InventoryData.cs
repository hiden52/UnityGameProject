using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="InventoryData", menuName ="Game Data/Inventory Data")]
public class InventoryData : ScriptableObject
{
    private const int MAX_SLOT_COUNT = 50;
    private int availableSlotCount;
    public int MaxSlotCount => MAX_SLOT_COUNT;
    public int AvailableSlotCount => availableSlotCount;
    public List<Item> items = new List<Item>();

    public void SetCurrentSlotCount(int slotCount)
    {
        availableSlotCount = slotCount;
    }
    
}
