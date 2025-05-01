using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public enum SlotContainerType
{
    None,
    Inventory,
    QuickSlot,
    EquipmentSlot,
}
public class SlotUI : MonoBehaviour, IPointerDownHandler, IItemSlot
{
    [SerializeField] private SlotContainerType slotContainerType = SlotContainerType.None;
    [SerializeField] Image itemIcon;
    [SerializeField] GameObject text;
    [SerializeField] int quantity = 0;
    private int slotIndex = -1;
    private Item currentItem;
    [SerializeField] private Image backroundImage;
    [SerializeField] private Canvas canvas;
    [SerializeField] private SlotUIHoverHandler hoverHandler;
    [SerializeField] private SlotUIDragHandler dragHandler;
    

    public SlotContainerType ContainerType => slotContainerType;

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

    public void SetContainerType(SlotContainerType container)
    {
        slotContainerType = container;
    }
    public void SetSlot(Item item)
    {
        if (slotContainerType == SlotContainerType.None)
        {
            Debug.LogError("[SlotUI] SlotContainerType is None. Set SlotContainerType!");
            return;
        }
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
