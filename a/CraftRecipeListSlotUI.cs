using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CraftRecipeHoverHandler))]
public class CraftRecipeListSlotUI : SlotUI, IPointerDownHandler
{
    [SerializeField] ItemRecipeData itemRecipeData;
    [SerializeField] CraftRecipeHoverHandler hoverHandler;
    private Text nameText;
    private Action<ItemRecipeData> onRecipeSelected;

    protected override void Awake()
    {
        base.Awake();
        nameText = text.GetComponent<Text>();
        hoverHandler = GetComponent<CraftRecipeHoverHandler>();

        if(hoverHandler != null )
        {
            hoverHandler.Initialize(backroundImage);
        }
    }

    public void Initialize(ItemRecipeData recipe, Action<ItemRecipeData> onSelected)
    {
        itemRecipeData = recipe;
        onRecipeSelected = onSelected;
        SetSlot();
    }
    public override void SetSlot()
    {
        if (itemRecipeData != null)
        {
            nameText.text = itemRecipeData.recipeName;
            nameText.gameObject.SetActive(true);
            icon.sprite = itemRecipeData.recipeIcon;
        }
        else
        {
            Debug.LogError($"{this.name} : Can't SetSlot(), itemRecipeData is null", this);
            nameText.text = string.Empty;
            icon.enabled = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (itemRecipeData != null && onRecipeSelected != null)
        {
            onRecipeSelected.Invoke(itemRecipeData);
        }
    }

}
