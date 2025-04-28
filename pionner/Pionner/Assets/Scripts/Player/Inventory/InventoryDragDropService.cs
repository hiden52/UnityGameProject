using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryDragDropService : MonoBehaviour
{
    [SerializeField] private Image draggedItemImage;
    [SerializeField] private Text draggedItemQuantityText; 
    [SerializeField] private Canvas parentCanvas;

    private Item draggedItem = null;
    private SlotUI sourceSlot = null;
    private bool isDragging = false;
    private RectTransform draggedItemRectTransform;

    void Awake()
    {
        if (parentCanvas == null) parentCanvas = GetComponentInParent<Canvas>();
        if (draggedItemQuantityText == null) draggedItemQuantityText = draggedItemImage.GetComponentInChildren<Text>();
        if (draggedItemImage != null) draggedItemRectTransform = draggedItemImage.GetComponent<RectTransform>();

        // init draggedIcon
        if (draggedItemImage != null) draggedItemImage.gameObject.SetActive(false);
        if(draggedItemQuantityText != null) draggedItemQuantityText.enabled = false;
    }


    public void StartDrag(SlotUI slot, PointerEventData eventData)
    {
        if (!slot.HasItem() || isDragging) return; 

        sourceSlot = slot;
        draggedItem = slot.GetItem();
        isDragging = true;

        draggedItemImage.sprite = draggedItem.Data.icon;
        draggedItemImage.color = new Color(1, 1, 1, 0.7f);
        draggedItemImage.gameObject.SetActive(true);
        //draggedItemImage.SetNativeSize();

        if(draggedItem is CountableItem countableItem)
        {
            draggedItemQuantityText.text = countableItem.currentStack.ToString();
            draggedItemQuantityText.enabled = true;
        }
        else
        {
            draggedItemQuantityText.enabled = false;
        }

        CanvasGroup sourceGroup = sourceSlot.GetComponent<CanvasGroup>();
        if (sourceGroup != null) sourceGroup.alpha = 0.6f;

        // 초기 위치 설정
        UpdateDraggedItemPosition(eventData);
    }

    public void UpdateDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        UpdateDraggedItemPosition(eventData);
    }

    public void EndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        HandleDrop(eventData);

        if (sourceSlot != null)
        {
            // 원본 슬롯 모습 복원
            CanvasGroup sourceGroup = sourceSlot.GetComponent<CanvasGroup>();
            if (sourceGroup != null)
            {
                sourceGroup.alpha = 1f;
            }
        }

        if (draggedItemImage != null)
        {
            draggedItemImage.gameObject.SetActive(false);
        }
        draggedItem = null;
        sourceSlot = null;
        isDragging = false;
    }


    private void UpdateDraggedItemPosition(PointerEventData eventData)
    {
        if (draggedItemRectTransform != null)
        {
            draggedItemRectTransform.position = eventData.position;
        }
    }

    private void HandleDrop(PointerEventData eventData)
    {
        SlotUI targetSlot = null;

        if (eventData.pointerCurrentRaycast.isValid)
        {
            GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;
            targetSlot = hitObject.GetComponentInParent<SlotUI>();
        }

        if (targetSlot != null && targetSlot != sourceSlot)
        {
            Debug.Log($"[InventoryDragDropService] 아이템 {draggedItem.Data.itemName} 을(를) 슬롯 {sourceSlot.GetSlotIndex()} 에서 슬롯 {targetSlot.GetSlotIndex()} 위로 드롭함");

            int sourceIndex = sourceSlot.GetSlotIndex();
            int targetIndex = targetSlot.GetSlotIndex();

            if (sourceIndex != -1 && targetIndex != -1)
            {
                InventoryManager.Instance.MoveOrSwapItem(sourceIndex, targetIndex); // InventoryManager에 구현 필요
            }
            else
            {
                Debug.LogError("[InventoryDragDropService] 드래그/드롭을 위한 슬롯 인덱스를 결정할 수 없습니다.");
            }
        }
        else
        {
            Debug.Log("[InventoryDragDropService] 유효한 슬롯 외부 또는 원래 슬롯 위에 드롭함.");
        }
    }

    public bool IsDragging() => isDragging;
    public Item GetDraggedItem() => draggedItem;
    public SlotUI GetSourceSlot() => sourceSlot;
}