using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUIHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image image;
    private static readonly Color DefaultColor = new Color(0.8490566f, 0.8490566f, 0.8490566f);
    protected Color hoverColor;

    public void Initialize(Image img)
    {
        image = img;
        hoverColor = Color.yellow;
    }

    private void OnEnable()
    {
        if(image != null)
        {
            ResetImage();
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        image.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetImage();
    }

    public virtual void ResetImage()
    {
        image.color = DefaultColor;
    }
}
