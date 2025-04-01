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
        // 슬롯 초기화
        InitializeSlots();
        // 초기화 후 인벤토리UI 갱신
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

        // 현재 활성화된 슬롯 수
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
            // 활성화된 슬롯 수 만큼만 활성화
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
