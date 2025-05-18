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
        // ������Ʈ ���� �ʱ�ȭ
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

        // �ǹ��� ������ ����� UI�� ����
        if (recipeListController && currentBuilding != null)
        {
            recipeListController.SetupRecipeList(currentBuilding.availableRecipes);
        }

        // ���õ� ������ �ʱ�ȭ
        selectedRecipe = null;
        if (craftInfoController)
        {
            craftInfoController.ResetInfo();
        }
    }

    public void SelectRecipe(ItemRecipeData recipeData)
    {
        selectedRecipe = recipeData;

        // ���õ� ������ ������ CraftInfoUI�� ����
        if (craftInfoController && selectedRecipe != null)
        {
            craftInfoController.SetBuildingInfo(selectedRecipe);
        }
    }
}
