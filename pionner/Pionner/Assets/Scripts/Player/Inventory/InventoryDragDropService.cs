using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryDragDropService : MonoBehaviour
{
    [SerializeField] private Image draggedItemImage;
    [SerializeField] private Text draggedItemQuantityText;
    [SerializeField] private Canvas parentCanvas;
    [Header("Manager references")]
    [SerializeField] private GameObject goInventoryActions;
    [SerializeField] private GameObject goQuickSlotActions;

    private IInventoryActions inventoryActions;
    private IQuickSlotActions quickSlotActions;

    private Item draggedItem = null;
    private SlotUI sourceSlot = null;
    private bool isDragging = false;
    private RectTransform draggedItemRectTransform;

    void Awake()
    {
        if (goInventoryActions != null)
        {
            inventoryActions = goInventoryActions.GetComponent<IInventoryActions>();
            if (inventoryActions == null)
            {
                string assignedObjectName = goInventoryActions.name;

                Debug.LogError($"[{GetType().Name}] Assigned GameObject '{assignedObjectName}' is missing the IInventoryActions component.", this);
            }
        }
        if (goQuickSlotActions != null)
        {
            quickSlotActions = goQuickSlotActions.GetComponent<IQuickSlotActions>();
            if (quickSlotActions == null)
            {
                string assignedObjectName = goQuickSlotActions.name;

                Debug.LogError($"[{GetType().Name}] Assigned GameObject '{assignedObjectName}' is missing the IInventoryActions component.", this);
            }
        }
        if (parentCanvas == null) parentCanvas = GetComponentInParent<Canvas>();
        if (draggedItemQuantityText == null) draggedItemQuantityText = draggedItemImage.GetComponentInChildren<Text>();
        if (draggedItemImage != null) draggedItemRectTransform = draggedItemImage.GetComponent<RectTransform>();

        // init draggedIcon
        if (draggedItemImage != null) draggedItemImage.gameObject.SetActive(false);
        if (draggedItemQuantityText != null) draggedItemQuantityText.enabled = false;
    }


    public void StartDrag(SlotUI slot, PointerEventData eventData)
    {
        if (inventoryActions == null || quickSlotActions == null)
        {
            Debug.LogError("[InventoryDragDropService] Cannot start drag: Manager references are not set!");
            return;
        }
        if (!slot.HasItem() || isDragging) return;

        sourceSlot = slot;
        draggedItem = slot.GetItem();
        isDragging = true;

        draggedItemImage.sprite = draggedItem.Data.icon;
        draggedItemImage.color = new Color(1, 1, 1, 0.7f);
        draggedItemImage.gameObject.SetActive(true);

        if (draggedItem is CountableItem countableItem && countableItem.currentStack > 1)
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

        HandleDrop(eventData); // 주의

        CleanUpDragState();
    }

    private void CleanUpDragState()
    {
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
        if (draggedItemQuantityText != null)
        {
            draggedItemQuantityText.enabled = false;
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

    // 05-08 Thu.
    // if-elseif 로 가독성 최악
    // 해결해야함.
    private void HandleDrop(PointerEventData eventData)
    {
        SlotUI targetSlot = null;
        GameObject hitObject = null;

        if (eventData.pointerCurrentRaycast.isValid)
        {
            hitObject = eventData.pointerCurrentRaycast.gameObject;
            targetSlot = hitObject.GetComponentInParent<SlotUI>();
        }

        if (targetSlot != null && sourceSlot != null)
        {
            if(targetSlot == sourceSlot)
            {
                Debug.Log("같은 슬롯에 드랍");
                return;
            }

            //Debug.Log($"[InventoryDragDropService] 아이템 {draggedItem.Data.itemName} 을(를) 슬롯 {sourceSlot.GetSlotIndex()} 에서 슬롯 {targetSlot.GetSlotIndex()} 위로 드롭함");

            SlotContainerType sourceType = sourceSlot.ContainerType;
            SlotContainerType targetType = targetSlot.ContainerType;
            int sourceIndex = sourceSlot.GetSlotIndex();
            int targetIndex = targetSlot.GetSlotIndex();

            Debug.Log($"Drop Interaction: {sourceType} Slot {sourceIndex} -> {targetType} Slot {targetIndex}");
            if (sourceType == SlotContainerType.Inventory && targetType == SlotContainerType.Inventory)
            {
                inventoryActions.MoveOrSwapItem(sourceIndex, targetIndex);
            }
            else if (sourceType == SlotContainerType.Inventory && targetType == SlotContainerType.QuickSlot)
            {
                if (draggedItem != null)
                {
                    quickSlotActions.AssignItem(targetIndex, draggedItem);
                }
            }
            else if (sourceType == SlotContainerType.Inventory && targetType == SlotContainerType.EquipmentSlot)
            {
                if(draggedItem != null)
                {
                    // 05-08 Thu.
                    // 현재 EquipmentsUI에서 대상 Slot이 어떤 EquipmentWhere을 관리하는지 알 수 없음
                    // 1. SlotUI에 EquipmentWhere 필드 추가하기
                    // 2. SlotUI 기능을 인터페이스로 빼서 EquipmentSlotUI를 만들고 상속받기
                    // 3. EquipmentUIController를 참조해서 해당 슬롯의 EquipmentWhere 찾기
                    if(draggedItem is EquipmentItem equipment)
                    {
                        equipment.Use();
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else if (sourceType == SlotContainerType.QuickSlot && targetType == SlotContainerType.Inventory)
            {
                Debug.Log("Unssign QuickSlot" + sourceIndex);
                quickSlotActions.UnassignItem(sourceIndex);
            }
            else if (sourceType == SlotContainerType.QuickSlot && targetType == SlotContainerType.QuickSlot)
            {
                quickSlotActions.MoveOrSwapAssignment(sourceIndex, targetIndex);
            }
            else if(sourceType == SlotContainerType.EquipmentSlot && targetType == SlotContainerType.Inventory)
            {
                draggedItem.Use();
            }
            else
            {
                Debug.LogWarning($"[InventoryDragDropService] Unhandled Slot Drop: {sourceType} -> {targetType}");
            }
        }
        else
        {
            SlotContainerType sourceType = sourceSlot.ContainerType;
            int sourceIndex = sourceSlot.GetSlotIndex();
            if (hitObject != null && hitObject.layer == 5)
            {
                switch (sourceType)
                {
                    case SlotContainerType.QuickSlot:
                        if (hitObject.tag == "Inventory UI")
                        {
                            quickSlotActions.UnassignItem(sourceIndex);
                        }
                        break;
                    case SlotContainerType.EquipmentSlot:
                        if(hitObject.tag == "Inventory UI")
                        {
                            draggedItem.Use();
                        }
                        break;
                    default:
                        return;
                }
            }
            else
            {
                switch (sourceType)
                {
                    case SlotContainerType.Inventory:
                        Debug.Log($"[InventoryDragDropService] 인벤토리 {sourceIndex} 외부 드롭, 아이템 버리기?");
                        inventoryActions.RemoveItemAtIndex(sourceIndex);
                        // 아이템 드롭 효과?
                        break;
                    case SlotContainerType.QuickSlot:
                        Debug.Log($"[InventoryDragDropService] 퀵슬롯 {sourceIndex} 외부 드롭, 할당 해제.");
                        quickSlotActions.UnassignItem(sourceIndex);
                        break;
                    case SlotContainerType.EquipmentSlot:
                        Debug.Log($"[InventoryDragDropService] 장비창 {sourceIndex} 외부 드롭, 장착 해제.");
                        if(draggedItem is EquipmentItem)
                        {
                            draggedItem.Use();
                        }
                        break;
                    default:
                        Debug.Log("유효하지 않은 대상에 드롭됨.");
                        break;
                }
            }
            
            
        }
    }

    public bool IsDragging() => isDragging;
    public Item GetDraggedItem() => draggedItem;
    public SlotUI GetSourceSlot() => sourceSlot;
}