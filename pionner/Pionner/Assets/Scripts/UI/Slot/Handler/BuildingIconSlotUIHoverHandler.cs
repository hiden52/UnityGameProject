using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingIconSlotUIHoverHandler : SlotUIHoverHandler
{
    
    private static Color BuildingIconDefaultColor = new Color(1, 1, 1, 0);

    public event Action OnHover;

    public void Initialize(Image img, Color color)
    {
        base.Initialize(img);
        hoverColor = color;
    }

    public override void ResetImage()
    {
        image.color = BuildingIconDefaultColor;
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        OnHover?.Invoke();
    }
}
