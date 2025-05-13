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

    private Color selectedColor = new Color(1, 0.6392157f, 0f);
    protected override void Awake()
    {
        base.Awake();

        buildingName = text.GetComponent<Text>();
        buildingRecipeData = null;
    }

    public void SetSlot(BuildingRecipeData buildingRecipe)
    {
        buildingRecipeData = buildingRecipe;
        if (buildingRecipeData == null) return;

        Debug.Log(buildingRecipeData.recipeName);
        buildingName.text = buildingRecipeData.recipeName;
        this.icon.sprite = buildingRecipeData.recipeIcon;
        text.SetActive(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        if (buildingRecipeData == null) return;
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log(selectedColor);
            backroundImage.color = selectedColor;
        }


    }
}
