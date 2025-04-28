using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class SlotUIDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private SlotUI slot;
    [SerializeField] private InventoryDragDropService inventoryDragDropService;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
        slot = GetComponent<SlotUI>();

        if(inventoryDragDropService == null)
        {
            inventoryDragDropService = GetComponentInParent<InventoryDragDropService>();
        }
    }

    // 드래그 이벤트
    public void OnBeginDrag(PointerEventData eventData)
    {
        inventoryDragDropService.StartDrag(slot, eventData);



    }
    public void OnDrag(PointerEventData eventData)
    {
        inventoryDragDropService.UpdateDrag(eventData);
       
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        inventoryDragDropService.EndDrag(eventData);
    }
}
