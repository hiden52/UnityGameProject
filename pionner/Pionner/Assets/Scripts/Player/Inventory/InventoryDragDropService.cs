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

        // �ʱ� ��ġ ����
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

        HandleDrop(eventData); // ����

        CleanUpDragState();
    }

    private void CleanUpDragState()
    {
        if (sourceSlot != null)
        {
            // ���� ���� ��� ����
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

    private void HandleDrop(PointerEventData eventData)
    {
        SlotUI targetSlot = null;

        if (eventData.pointerCurrentRaycast.isValid)
        {
            GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;
            targetSlot = hitObject.GetComponentInParent<SlotUI>();
        }

        if (targetSlot != null && sourceSlot != null)
        {
            if(targetSlot == sourceSlot)
            {
                Debug.Log("���� ���Կ� ���");
                return;
            }

            //Debug.Log($"[InventoryDragDropService] ������ {draggedItem.Data.itemName} ��(��) ���� {sourceSlot.GetSlotIndex()} ���� ���� {targetSlot.GetSlotIndex()} ���� �����");

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
            //else if (sourceType == SlotContainerType.Inventory && targetType == SlotContainerType.EquipmentSlot)
            else if (sourceType == SlotContainerType.QuickSlot && targetType == SlotContainerType.Inventory)
            {
                Debug.Log("Unssign QuickSlot" + sourceIndex);
                quickSlotActions.UnassignItem(sourceIndex);
            }
            else if (sourceType == SlotContainerType.QuickSlot && targetType == SlotContainerType.QuickSlot)
            {
                quickSlotActions.MoveOrSwapAssignment(sourceIndex, targetIndex);
            }
            //else if(sourceType == SlotContainerType.EquipmentSlot && targetType == SlotContainerType.Inventory)
            else
            {
                Debug.LogWarning($"[InventoryDragDropService] Unhandled Slot Drop: {sourceType} -> {targetType}");
            }
        }
        else
        {
            SlotContainerType sourceType = sourceSlot.ContainerType;
            int sourceIndex = sourceSlot.GetSlotIndex();

            if (sourceType == SlotContainerType.QuickSlot)
            {
                Debug.Log($"[InventoryDragDropService] ������ {sourceIndex} �ܺ� ���, �Ҵ� ����.");
                quickSlotActions.UnassignItem(sourceIndex);
            }
            else if (sourceType == SlotContainerType.Inventory)
            {
                Debug.Log($"[InventoryDragDropService] �κ��丮 {sourceIndex} �ܺ� ���, ������ ������?");
                inventoryActions.RemoveItemAtIndex(sourceIndex);
                // ������ ��� ȿ��?
            }
            else
            {
                Debug.Log("��ȿ���� ���� ��� ��ӵ�.");
            }
        }
    }

    public bool IsDragging() => isDragging;
    public Item GetDraggedItem() => draggedItem;
    public SlotUI GetSourceSlot() => sourceSlot;
}