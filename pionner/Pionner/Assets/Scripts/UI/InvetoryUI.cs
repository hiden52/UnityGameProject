using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvetoryUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotPannel;
    public InventoryManager inventory;

    private List<SlotUI> slots = new List<SlotUI>();
    private int defaultSlotNum = 20;

    private void Start()
    {
        for (int i = 0; i < defaultSlotNum; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotPannel);
            SlotUI slotUI = slotObj.GetComponent<SlotUI>();
            slots.Add(slotUI);
        }

        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        for(int i = 0; i < slots.Count;i++)
        {
            if(i < InventoryManager.Instance.Items.Count)
            {
                slots[i].SetSlot(InventoryManager.Instance.Items[i]);
            }
            else
            {
                slots[i].SetSlot(null);
            }
        }
    }
}
