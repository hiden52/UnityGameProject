using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUIHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image image;
    private static readonly Color DefaultColor = new Color(0.8490566f, 0.8490566f, 0.8490566f);

    public void Initialize(Image img)
    {
        image = img;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = DefaultColor;
    }
}
