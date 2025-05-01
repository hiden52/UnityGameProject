using System;

public interface IQuickSlotActions
{
    Item GetItem(int index);
    void AssignItem(int quickSlotIndex, Item itemToAssign);
    void UnassignItem(int quickSlotIndex);
    void MoveOrSwapAssignment(int sourceIndex, int targetIndex);

    event Action<int, Item> OnQuickSlotChanged;
}