using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    [SerializeField] Image itemIcon;
    [SerializeField] GameObject text;
    [SerializeField] int quantity = 0;

    private void Awake()
    {
        text.SetActive(false);
    }
    private void Start()
    {
        itemIcon = transform.GetChild(0).GetComponent<Image>();
    }

    // 이미지의 알파값 수정
    private void SetAlpha(float alpha)
    {
        Color colorIcon = itemIcon.color;
        colorIcon.a = alpha;
        itemIcon.color = colorIcon;
    }

    public void SetSlot(Item item)
    {
        if (item != null)
        {
            itemIcon.sprite = item.data.icon;
            SetAlpha(1f);
        }
        else
        {
            ClearSlot();
        }
    }
    public void SetSlot(CountableItem item)
    {
        if (item != null)
        {
            itemIcon.sprite = item.data.icon;
            SetAlpha(1f);
            quantity = item.currentStack;

            text.SetActive(true);
            text.GetComponent<TextMeshProUGUI>().SetText(quantity.ToString());
        }
        else
        {
            ClearSlot();            
        }
    }

    public void ClearSlot()
    {
        itemIcon.sprite = null;
        SetAlpha(0);
        text.SetActive(false);

    }
    
}
