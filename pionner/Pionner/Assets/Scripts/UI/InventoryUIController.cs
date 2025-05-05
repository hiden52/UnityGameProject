using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotPannel;
    [SerializeField] private InventoryData inventoryData;

    private List<SlotUI> slots = new List<SlotUI>();
    private void OnEnable()
    {
        // �ʱ�ȭ ���� �ʾҴٸ� �ʱ�ȭ
        if(slots.Count <= 0)
        {
            InitializeSlots();
           
        }

        UpdateInventoryUI(inventoryData.items);
        InventoryManager.OnInventoryChanged += UpdateInventoryUI;
        InventoryManager.Instance.OnSlotUpdated += UpdateInventorySlot;
    }

    public void OnDisable()
    {
        InventoryManager.OnInventoryChanged -= UpdateInventoryUI;
        InventoryManager.Instance.OnSlotUpdated -= UpdateInventorySlot;
    }

    private void InitializeSlots()
    {
        slots.Clear();
        for (int i = 0; i < inventoryData.MaxSlotCount; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotPannel);
            slotObj.name = $"Slot_{i}";
            SlotUI slotUI = slotObj.GetComponent<SlotUI>();
            slotUI.SetContainerType(SlotContainerType.Inventory);
            slotUI.SetSlotIndex(i);
            slotUI.SetSlot(null);
            slots.Add(slotUI);
            slotObj.SetActive(false);
        }
    }

    private void UpdateInventorySlot(int index, Item item)
    {
        if (index >= 0 && index < slots.Count)
        {
            slots[index].SetSlot(item);
        }
    }
    public void UpdateInventoryUI(List<Item> items)
    {
        // Debug
        if(Application.isEditor)
        {
            //Debug.Log("Updata Inventory UI");
            //Debug.Log("items count: " +  items.Count);
        }

        // ���� Ȱ��ȭ�� ���� ��
        int activeSlotCount = inventoryData.AvailableSlotCount;
        //Debug.Log("activeSlotCout : " + activeSlotCount);

        if (items.Count <= 0)
        {
            for(int i = 0; i < activeSlotCount;i++)
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

        for (int i = 0; i < slots.Count; i++)
        {
            // Ȱ��ȭ�� ���� �� ��ŭ�� Ȱ��ȭ
            if(i < activeSlotCount)
            {
                slots[i].gameObject.SetActive(true);
            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }
}
