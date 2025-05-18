using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRecipeForwarder : MonoBehaviour
{
    [SerializeField] private BuildingData currentBuilding;
    [SerializeField] private ItemRecipeData selectedRecipe;

    [SerializeField] private GameObject recipeListGO;
    [SerializeField] private GameObject craftInfoGO;

    [SerializeField] private CraftRecipeListController recipeListController;
    [SerializeField] private CraftInfoUIController craftInfoController;

    public ItemRecipeData SelectedRecipe => selectedRecipe;
    private void Awake()
    {
        // 컴포넌트 참조 초기화
        if (!recipeListController && recipeListGO)
        {
            recipeListController = recipeListGO.GetComponent<CraftRecipeListController>();
        }

        if (!craftInfoController && craftInfoGO)
        {
            craftInfoController = craftInfoGO.GetComponent<CraftInfoUIController>();
        }
    }
    public void SetCurrentBuilding(BuildingData buildingData)
    {
        currentBuilding = buildingData;

        // 건물의 레시피 목록을 UI에 전달
        if (recipeListController && currentBuilding != null)
        {
            recipeListController.SetupRecipeList(currentBuilding.availableRecipes);
        }

        // 선택된 레시피 초기화
        selectedRecipe = null;
        if (craftInfoController)
        {
            craftInfoController.ResetInfo();
        }
    }

    public void SelectRecipe(ItemRecipeData recipeData)
    {
        selectedRecipe = recipeData;

        // 선택된 레시피 정보를 CraftInfoUI에 전달
        if (craftInfoController && selectedRecipe != null)
        {
            craftInfoController.SetBuildingInfo(selectedRecipe);
        }
    }
}
