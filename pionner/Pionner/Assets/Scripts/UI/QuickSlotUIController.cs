using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotUIController : MonoBehaviour
{
    // 2025-04-30 ∏Æ∆—≈Õ∏µ
    [SerializeField] QuickSlotManager quickSlotManager;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotPannel;
    [SerializeField] private Item[] items;
    private List<SlotUI> slots = new List<SlotUI>();
    private int MAX_SLOT_COUNT;


    private void Awake()
    {
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
        if (slots.Count <= 0)
        {
            InitializeSlots();

        }

        UpdateQuickSlotUI();
    }

    public void OnDisable()
    {
    }

    private void InitializeSlots()
    {
        slots.Clear();
        for (int i = 0; i < MAX_SLOT_COUNT; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotPannel);
            slotObj.name = $"QuickSlot_{i}";
            SlotUI slotUI = slotObj.GetComponent<SlotUI>();
            slotUI.SetContainerType(SlotContainerType.QuickSlot);
            slotUI.SetSlotIndex(i);
            slotUI.SetSlot(null);
            slots.Add(slotUI);
        }
    }

    public void UpdateQuickSlotUI()
    {
        
        for (int i = 0; i < MAX_SLOT_COUNT; i++)
        {
            slots[i].SetSlot(items[i]);
        }
    }

}
