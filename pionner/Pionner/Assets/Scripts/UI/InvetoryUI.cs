using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvetoryUI : MonoBehaviour
{
    [SerializeField] GameObject slotPrefab;
    [SerializeField] Transform slotPannel;

    private List<SlotUI> slots = new List<SlotUI>();
    private int DEFAULT_SLOT_COUNT = 24;

    private void Start()
    {
        UpdateInventoryUI(InventoryManager.Instance.Items);
    }

    private void OnEnable()
    {
        InventoryManager.OnInventoryChanged += UpdateInventoryUI;
    }

    public void OnDisable()
    {
        InventoryManager.OnInventoryChanged -= UpdateInventoryUI;
    }



    public void UpdateInventoryUI(List<Item> items)
    {
        // Clear Slots
        foreach(Transform child in slotPannel)
        {
            Destroy(child.gameObject);
        }
        slots.Clear();

        // Create Slot
        foreach(Item item in items)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotPannel);
            SlotUI slotUI = slotObj.GetComponent<SlotUI>();
            slotUI.SetSlot(item);
            slots.Add(slotUI);
        }

        while (slots.Count < DEFAULT_SLOT_COUNT)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotPannel);
            SlotUI slotUI = slotObj.GetComponent< SlotUI>();
            slotUI.SetSlot(null);
            slots.Add(slotUI);
        }
    }
}
