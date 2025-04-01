using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotPannel;
    [SerializeField] private InventoryData inventoryData;

    private List<SlotUI> slots = new List<SlotUI>();

    private void Awake()
    {
        
    }
    private void Start()
    {
        // ���� �ʱ�ȭ
        InitializeSlots();
        // �ʱ�ȭ �� �κ��丮UI ����
        UpdateInventoryUI(inventoryData.items);
    }

    private void OnEnable()
    {
        UpdateInventoryUI(inventoryData.items);
        InventoryManager.OnInventoryChanged += UpdateInventoryUI;
    }

    public void OnDisable()
    {
        InventoryManager.OnInventoryChanged -= UpdateInventoryUI;
    }

    private void InitializeSlots()
    {
        for (int i = 0; i < inventoryData.MaxSlotCount; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotPannel);
            SlotUI slotUI = slotObj.GetComponent<SlotUI>();
            slotUI.SetSlot(null);
            slots.Add(slotUI);
            slotObj.SetActive(false);
        }
    }


    public void UpdateInventoryUI(List<Item> items)
    {
        // Debug
        if(Application.isEditor)
        {
            //Debug.Log("Updata Inventory UI");
        }

        // ���� Ȱ��ȭ�� ���� ��
        int activeSlotCount = inventoryData.currentSlotCount;
        //Debug.Log("activeSlotCout : " + activeSlotCount);

        for(int i=0; i<items.Count; i++)
        {
            if(i < slots.Count)
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
