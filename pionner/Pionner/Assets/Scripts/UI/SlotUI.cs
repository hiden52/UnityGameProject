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

    // �̹����� ���İ� ����
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
            Debug.Log($"{CurrentItem.data.itemName} ���Կ� �߰���");
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
            Debug.Log($"{CurrentItem.data.itemName} ���Կ� �߰���");
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

    // Ŭ�� �̺�Ʈ
    public void OnPointerDown(PointerEventData eventData)
    {
        if (CurrentItem == null) return;
        Debug.Log($"{CurrentItem.data.itemName} Ŭ����!");

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            CurrentItem.Use();
        }
    }

    // �巡�� �̺�Ʈ
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
