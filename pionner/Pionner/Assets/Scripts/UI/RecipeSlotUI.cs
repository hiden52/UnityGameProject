using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RecipeSlotUI : SlotUI, IPointerDownHandler
{
    [SerializeField] private SlotUIHoverHandler hoverHandler;
    [SerializeField] private ItemData itemData;

    [Header("Amount UI")]
    [SerializeField] private Slider slider;
    [SerializeField] private int amountHaving;
    [SerializeField] private int amountRequired;
    


    protected override void Awake()
    {
        base.Awake();

        hoverHandler = GetComponent<SlotUIHoverHandler>();
        if (hoverHandler != null)
        {
            hoverHandler.Initialize(backroundImage);
        }
        amountHaving = 3;
    }

    protected void SetQuantity(int n)
    {
        quantity = n;

        text.SetActive(true);
        text.GetComponent<Text>().text = amountHaving + "/" + n.ToString();
    }
    private void SetSlider()
    {
        if(slider != null)
        {
            slider.enabled = true;
            slider.value = Mathf.Clamp01((float)amountHaving / amountRequired);
            
        }
    }

    public void SetSlot(RecipeIngredient _recipeIngredient)
    {
        if (_recipeIngredient.itemData != null)
        {
            itemData = _recipeIngredient.itemData;
            amountRequired = _recipeIngredient.amount;
            icon.sprite = itemData.icon;
            SetAlpha(1f);

            SetQuantity(amountRequired);
            SetSlider();
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
