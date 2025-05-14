using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RecipeSlotUI : SlotUI, IPointerDownHandler
{
    [SerializeField] private SlotUIHoverHandler hoverHandler;
    [SerializeField] private ItemData itemData;
    [SerializeField] private int amount;


    protected override void Awake()
    {
        base.Awake();

        hoverHandler = GetComponent<SlotUIHoverHandler>();
        if (hoverHandler != null)
        {
            hoverHandler.Initialize(backroundImage);
        }
    }

    protected void SetQuantity(int n)
    {
        quantity = n;

        text.SetActive(true);
        text.GetComponent<Text>().text = n.ToString();
    }

    public void SetSlot(RecipeIngredient _recipeIngredient)
    {
        if (_recipeIngredient.itemData != null)
        {
            itemData = _recipeIngredient.itemData;
            amount = _recipeIngredient.amount;
            icon.sprite = itemData.icon;
            SetAlpha(1f);

            if (itemData is CountableItemData)
            {
                SetQuantity(amount);
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

    public void OnPointerDown(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log($"{itemData.itemName} Right Button Click!");
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log($"{itemData.itemName} Left Button Click!");
        }

    }
}
