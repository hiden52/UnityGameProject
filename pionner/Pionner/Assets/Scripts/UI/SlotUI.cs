using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class SlotUI : MonoBehaviour
{
    [SerializeField] protected Image itemIcon;
    [SerializeField] protected GameObject text;
    [SerializeField] protected int quantity = 0;
    [SerializeField] protected Image backroundImage;

    protected virtual void Awake()
    {
        if (text != null)
        {
            text.SetActive(false);
        }
        backroundImage = GetComponent<Image>();
    }

    // 이미지의 알파값 수정
    protected void SetAlpha(float alpha)
    {
        Color colorIcon = itemIcon.color;
        colorIcon.a = alpha;
        itemIcon.color = colorIcon;
    }


    public virtual void ClearSlot()
    {
        itemIcon.sprite = null;
        SetAlpha(0);
        text.SetActive(false);

    }
    public virtual void SetSlot()
    {

    }
}
