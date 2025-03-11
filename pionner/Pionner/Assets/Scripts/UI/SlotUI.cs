using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    [SerializeField] Image itemIcon = null;
    [SerializeField] GameObject text;
    [SerializeField] int quantity = 0;

    private void Awake()
    {
        text.SetActive(false);
    }

    public void SetSlot(Item item)
    {
        if (item != null)
        {
            itemIcon.sprite = item.data.icon;
        }
        else
        {
            itemIcon = null;
        }
    }
    public void SetSlot(CountableItem item)
    {
        if (item != null)
        {
            itemIcon.sprite = item.data.icon;
            quantity = item.currentStack;

            text.SetActive(true);
            text.GetComponent<TextMeshProUGUI>().SetText(quantity.ToString());
        }
        else
        {
            itemIcon = null;
        }
    }

    public void ClearSlot()
    {
        itemIcon.sprite = null;
        text.SetActive(false);
    }
    
}
