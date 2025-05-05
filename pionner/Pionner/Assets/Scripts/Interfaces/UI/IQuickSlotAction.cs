using System;

public interface IQuickSlotActions
{
    int GetMaxCount();
    Item GetItem(int index);
    Item[] GetItems();
    void AssignItem(int quickSlotIndex, Item itemToAssign);
    void UnassignItem(int quickSlotIndex);
    void MoveOrSwapAssignment(int sourceIndex, int targetIndex);
    public int FindQuickSlotIndex(Item item);

    event Action<int, Item> OnQuickSlotChanged;
}