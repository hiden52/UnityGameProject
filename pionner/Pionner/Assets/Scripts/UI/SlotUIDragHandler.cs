using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class SlotUIDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private IItemSlot slot;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
        slot = GetComponent<IItemSlot>();
    }

    // 드래그 이벤트
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!slot.HasItem()) return;
        group.blocksRaycasts = false;

    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!slot.HasItem()) return;
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!slot.HasItem()) return;
        group.blocksRaycasts = true;
    }
}
