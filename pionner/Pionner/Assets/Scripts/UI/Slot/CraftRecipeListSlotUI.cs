using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CraftRecipeHoverHandler))]
public class CraftRecipeListSlotUI : SlotUI, IPointerDownHandler
{
    [SerializeField] ItemRecipeData itemRecipeData;
    [SerializeField] CraftRecipeHoverHandler hoverHandler;
    private Text nameText;


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
    public override void SetSlot()
    {
        if (itemRecipeData != null)
        {
            nameText.text = itemRecipeData.recipeName;
            icon.sprite = itemRecipeData.recipeIcon;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // craftInfo
    }

}
