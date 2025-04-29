using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotUIController : MonoBehaviour
{
    [SerializeField] QuickSlotManager quickSlotManager;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotPannel;
    [SerializeField] private List<Item> items;
    private List<SlotUI> slots = new List<SlotUI>();
    private int MAX_SLOT_COUNT;


    private void Awake()
    {
        items = new List<Item>();
        if (quickSlotManager != null)
        {
            MAX_SLOT_COUNT = quickSlotManager.MAX_SLOT_COUNT;
            items = quickSlotManager.Items;
        }
    }

    private void Start()
    {
        
    }
    private void OnEnable()
    {
        // 초기화 되지 않았다면 초기화
        if (slots.Count <= 0)
        {
            InitializeSlots();

        }

        UpdateInventoryUI(items);
        //InventoryManager.OnInventoryChanged += UpdateInventoryUI;
    }

    public void OnDisable()
    {
        //InventoryManager.OnInventoryChanged -= UpdateInventoryUI;
    }

    private void InitializeSlots()
    {
        slots.Clear();
        for (int i = 0; i < MAX_SLOT_COUNT; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotPannel);
            slotObj.name = $"QuickSlot_{i}";
            SlotUI slotUI = slotObj.GetComponent<SlotUI>();
            slotUI.SetSlotIndex(i);
            slotUI.SetSlot(null);
            slots.Add(slotUI);
        }
    }

    public void UpdateInventoryUI(List<Item> items)
    {
        if (items.Count <= 0)
        {
            for (int i = 0; i < MAX_SLOT_COUNT; i++)
            {
                slots[i].SetSlot(null);
            }
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (i < slots.Count)
                {
                    if (items[i] is CountableItem countableItem)
                    {
                        slots[i].SetSlot(countableItem);
                    }
                    else
                    {
                        slots[i].SetSlot(items[i]);
                    }
                }
            }
        }

    }
}
