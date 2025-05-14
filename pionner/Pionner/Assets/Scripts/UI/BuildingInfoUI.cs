using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfoUI : MonoBehaviour
{
    [SerializeField] private Text buildingName;
    [SerializeField] private Image buildingIcon;
    [SerializeField] private Text buildingDescription;
    [SerializeField] private Transform buildingRecipeParent;
    [SerializeField] private RecipeSlotUI[] recipeSlotUIs;

    private void Awake()
    {
        buildingName.text = null;
        buildingDescription.text = null;
        recipeSlotUIs = buildingRecipeParent.GetComponentsInChildren<RecipeSlotUI>();
        SetIconAlpha(0);

        foreach (var slot in recipeSlotUIs)
        {
            slot.gameObject.SetActive(false);
        }
    }
    private void Start()
    {
       
       
    }

    private void SetIconAlpha(float alpha)
    {
        Color color = buildingIcon.color;
        color.a = alpha;
        buildingIcon.color = color;
    }

    private void ResetRecipeSlots()
    {
        foreach (var slot in recipeSlotUIs)
        {
            slot.gameObject.SetActive(false);
        }
    }

    public void SetBuildingInfo(BuildingRecipeData buildingRecipe)
    {
        ResetRecipeSlots();
        buildingName.text = buildingRecipe.recipeName;
        buildingIcon.sprite = buildingRecipe.recipeIcon;
        buildingDescription.text = buildingRecipe.description;

        foreach (var ingredient in buildingRecipe.ingredients.Select((item, index) => (item, index)))
        {
            recipeSlotUIs[ingredient.index].SetSlot(ingredient.item);
            recipeSlotUIs[ingredient.index].gameObject.SetActive(true);
        }
        SetIconAlpha(1);
    }


}
