using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Image itemIcon;
    [SerializeField] GameObject text;
    [SerializeField] int quantity = 0;
    public Item CurrentItem { get; private set; }
    [SerializeField] private Canvas canvas;

    private void Awake()
    {
        text.SetActive(false);
        canvas = GetComponentInParent<Canvas>();
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
        CurrentItem = item;

        if (CurrentItem != null)
        {
            Debug.Log($"{CurrentItem.data.itemName} 슬롯에 추가됨");
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
        CurrentItem = item;

        if (CurrentItem != null)
        {
            Debug.Log($"{CurrentItem.data.itemName} 슬롯에 추가됨");
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
        CurrentItem = null;
        itemIcon.sprite = null;
        SetAlpha(0);
        text.SetActive(false);

    }

    // 클릭 이벤트
    public void OnPointerDown(PointerEventData eventData)
    {
        if (CurrentItem == null) return;
        Debug.Log($"{CurrentItem.data.itemName} 클릭됨!");

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            CurrentItem.Use();
        }
    }

    // 드래그 이벤트
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (CurrentItem == null) return;
        
    }
    public void OnDrag(PointerEventData eventData)
    {
        if(CurrentItem == null) return;
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(CurrentItem == null) return;
    }
    
}
