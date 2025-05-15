using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class BuildingIconSlotUI : SlotUI, IPointerDownHandler
{
    private Text buildingName;
    private BuildingRecipeData buildingRecipeData;
    [SerializeField] private BuildingIconSlotUIHoverHandler hoverHandler;

    public event Action<BuildingRecipeData> OnIconClicked;
    public event Action<BuildingRecipeData> OnMouseHover;

    private Color hoverColor = new Color(1, 0.6392157f, 0f);
    protected override void Awake()
    {
        base.Awake();

        buildingName = text.GetComponent<Text>();
        buildingRecipeData = null;
        hoverHandler = GetComponent<BuildingIconSlotUIHoverHandler>();
        if(hoverHandler != null )
        {
            hoverHandler.Initialize(backroundImage, hoverColor);
            hoverHandler.OnHover += SetInfo;
        }
    }
    private void OnDestroy()
    {
        OnIconClicked = null;
        OnMouseHover = null;
    }
    public void SetInfo()
    {
        OnMouseHover?.Invoke(buildingRecipeData);
    }
    public void SetSlot(BuildingRecipeData buildingRecipe)
    {
        buildingRecipeData = buildingRecipe;
        if (buildingRecipeData == null) return;

        buildingName.text = buildingRecipeData.recipeName;
        this.icon.sprite = buildingRecipeData.recipeIcon;
        text.SetActive(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        if (buildingRecipeData == null) return;
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            OnIconClicked?.Invoke(buildingRecipeData);
        }


    }
}
