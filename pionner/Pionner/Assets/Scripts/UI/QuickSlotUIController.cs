using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuickSlotUIController : MonoBehaviour
{
    // 2025-04-30 ∏Æ∆—≈Õ∏µ
    [SerializeField] MonoBehaviour quickSlotManagerMB;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotPannel;
    [SerializeField] private Item[] items;
    private IQuickSlotActions quickSlotActions;
    private List<ItemSlotUI> slots = new List<ItemSlotUI>();
    private int MAX_SLOT_COUNT;


    private void Awake()
    {
        if (quickSlotManagerMB != null)
        {
            quickSlotActions = quickSlotManagerMB.GetComponent<IQuickSlotActions>();
            if(quickSlotActions != null )
            {
                MAX_SLOT_COUNT = quickSlotActions.GetMaxCount();
            }
            else
            {
                Debug.LogError("[QuickSlotUIController] Failed to set reference of QuickSlotManager");
            }
            
        }
        else
        {
            Debug.LogError("[QuickSlotUIController] Failed to set reference of QuickSlotManager Container");
        }
    }



    private void Start()
    {
        InitializeSlots();
        InitItems();
    }
    private void OnEnable()
    {
        quickSlotActions.OnQuickSlotChanged += UpdateQuickSlotUI;
    }

    public void OnDisable()
    {
        quickSlotActions.OnQuickSlotChanged -= UpdateQuickSlotUI;
    }

    private void InitializeSlots()
    {
        slots.Clear();
        for (int i = 0; i < MAX_SLOT_COUNT; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotPannel);
            slotObj.name = $"QuickSlot_{i}";
            ItemSlotUI slotUI = slotObj.GetComponent<ItemSlotUI>();
            slotUI.SetContainerType(SlotContainerType.QuickSlot);
            slotUI.SetSlotIndex(i);
            slotUI.SetSlot(null);
            slots.Add(slotUI);
        }
    }

    private void InitItems()
    {
        items = quickSlotActions.GetItems();
        if (items == null)
        {
            Debug.LogError("Items is null!!", this);
            return;
        }
        for (int i = 0; i < MAX_SLOT_COUNT; i++)
        {
            if (items[i] != null)
            {
                slots[i].SetSlot(items[i]);
            }
            
        }
    }
    public void UpdateQuickSlotUI(int index, Item item)
    {
        //Debug.Log($"Index : {index} , slots.Count : {slots.Count} item : {item.Data.itemName}");
        //return;
        slots[index].SetSlot(item);
    }

}
