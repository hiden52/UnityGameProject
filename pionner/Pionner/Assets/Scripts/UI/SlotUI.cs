using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class SlotUI : MonoBehaviour, IPointerDownHandler, IItemSlot
{
    [SerializeField] Image itemIcon;
    [SerializeField] GameObject text;
    [SerializeField] int quantity = 0;
    private Item currentItem;
    [SerializeField] private Image backroundImage;
    [SerializeField] private Canvas canvas;
    [SerializeField] private SlotUIHoverHandler hoverHandler;
    [SerializeField] private SlotUIDragHandler dragHandler;
    private int slotIndex = -1;

    private void Awake()
    {
        text.SetActive(false);
        canvas = GetComponentInParent<Canvas>();
        backroundImage = GetComponent<Image>();

        hoverHandler = GetComponent<SlotUIHoverHandler>();
        dragHandler = GetComponent<SlotUIDragHandler>();

        if (hoverHandler != null)
        {
            hoverHandler.Initialize(backroundImage);
        }
        
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
        currentItem = item;

        if (currentItem != null)
        {           
            itemIcon.sprite = item.Data.icon;
            SetAlpha(1f);

            if(item is CountableItem countable)
            {
                SetQuantity(countable.currentStack);
            }
            else
            {
                text.SetActive(false);
            }
        }
        else
        {
            ClearSlot();
        }
    }

    private void SetQuantity(int n)
    {
        quantity = n;

        text.SetActive(true);
        text.GetComponent<TextMeshProUGUI>().SetText(quantity.ToString());
    }
    public void ClearSlot()
    {
        currentItem = null;
        itemIcon.sprite = null;
        SetAlpha(0);
        text.SetActive(false);

    }

    // 클릭 이벤트
    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentItem == null) return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log($"{currentItem.Data.itemName} Right Button Click!");
            currentItem.Use();
        }
        else if(eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log($"{currentItem.Data.itemName} Left Button Click!");
        }

    }

    public bool HasItem() => currentItem != null;
    public Item GetItem() => currentItem;
    public int GetSlotIndex() => slotIndex;

    public void SetSlotIndex(int index) => slotIndex = index;

}
