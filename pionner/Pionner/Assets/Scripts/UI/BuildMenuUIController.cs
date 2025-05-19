using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuUIController : MonoBehaviour
{
    [Header("Data")]
    [Tooltip("건설 메뉴에 표시될 건물 카테고리 목록")]
    public List<BuildingCategoryData> buildingCategories;

    [Header("UI References")]
    public Transform categoryButtonParent; 
    public GameObject categoryButtonPrefab; 

    public Transform buildingListParent; // 선택된 카테고리의 건물 목록이 표시될 부모 Transform
    public GameObject buildingButtonPrefab; // 건물 버튼 프리팹 (RecipeData를 표시)

    [Header("Buiding Info")]
    public BuildingInfoUI buildingInfoUI;
    [SerializeField]
    private GameObject IngredientAlert;

    private BuildingCategoryData currentlySelectedCategory;
    //private BuildingRecipeData currentlySelectedRecipe;

    private void Awake()
    {
        currentlySelectedCategory = null;
        //currentlySelectedRecipe = null;
    }
    private void Start()
    {
        InitializeCategoryButtons();
        if (buildingCategories.Count <= 0) return;
        DisplayBuildingRecipesForCategory(buildingCategories[0]);
    }

    void InitializeCategoryButtons()
    {
        if (categoryButtonParent == null || categoryButtonPrefab == null || buildingCategories == null)
        {
            Debug.LogError("[BuildMenuUI] UI References or Data not set!");
            return;
        }

        // 기존 버튼들 삭제 (동적 생성이므로)
        foreach (Transform child in categoryButtonParent)
        {
            Destroy(child.gameObject);
        }

        // 카테고리 데이터만큼 버튼 생성
        foreach (BuildingCategoryData categoryData in buildingCategories)
        {
            GameObject buttonGO = Instantiate(categoryButtonPrefab, categoryButtonParent);
            Text nameComponent = buttonGO.gameObject.GetComponentInChildren<Text>();
            if (nameComponent != null)
            {
                nameComponent.text = categoryData.categoryName;
            }

            Button button = buttonGO.GetComponent<Button>();
            if (button != null)
            {
                BuildingCategoryData currentCategory = categoryData;
                button.onClick.AddListener(() => DisplayBuildingRecipesForCategory(currentCategory));
            }
        }
    }

    void DisplayBuildingRecipesForCategory(BuildingCategoryData categoryData)
    {
        if (buildingListParent == null || buildingButtonPrefab == null || categoryData == null)
        {
            Debug.LogError("[BuildMenuUI] UI References or Category Data not set for displaying buildings!");
            return;
        }

        foreach (Transform child in buildingListParent)
        {
            Destroy(child.gameObject);
        }

        foreach (BuildingRecipeData constructionRecipe in categoryData.buildingConstructionRecipes)
        {
            if (constructionRecipe.buildingToConstruct == null)
            {
                Debug.LogWarning($"Recipe '{constructionRecipe.name}' in category '{categoryData.name}' buildingToConstruct is null.");
                continue;
            }

            GameObject buildingIcon = Instantiate(buildingButtonPrefab, buildingListParent);
            BuildingIconSlotUI buildingIconSlot = buildingIcon.GetComponent<BuildingIconSlotUI>();
            if (buildingIconSlot != null)
            {
                buildingIconSlot.SetSlot(constructionRecipe);
                buildingIconSlot.OnIconClicked += SelectBuildingToConstruct;
                buildingIconSlot.OnMouseHover += ShowBuildingInfo;
            }
            
            
        }
        currentlySelectedCategory = categoryData;
        //currentlySelectedRecipe = null;
        ShowBuildingInfo(categoryData.buildingConstructionRecipes[0]);
    }

    private void ShowBuildingInfo(BuildingRecipeData constructionRecipe)
    {
        if (constructionRecipe != null)
        {
            UpdateBuildingInfo(constructionRecipe);
            //currentlySelectedRecipe = constructionRecipe;
        }
    }
    void SelectBuildingToConstruct(BuildingRecipeData constructionRecipe)
    {
        Debug.Log($"Start to Contruct {constructionRecipe.recipeName}.");
        Sprite icon = null;
        if (CanBuild())
        {
            BuildManager.Instance.StartBuildMode(constructionRecipe);
        }
        else
        {
            foreach (RecipeSlotUI slot in buildingInfoUI.RecipeSlotUIs)
            {
                if (!slot.gameObject.activeSelf) break;
                if (!slot.HasEnoughMaterials)
                {
                    icon = slot.Icon;
                    break;
                }
            }
            AlertManager.Instance.ShowIngredientAlert("재료가 부족합니다!", icon);
            Debug.Log("Can't Build. Not enough ingridients");
        }
        //currentlySelectedRecipe = null;


        // 두 번째 클릭 시에 BuildMode
        // BuildManager.Instance.StartPlacementMode(constructionRecipe.buildingToConstruct, constructionRecipe);
    }

    private void UpdateBuildingInfo(BuildingRecipeData constructionRecipe)
    {
        //Debug.Log("Update Building Recipe");
        buildingInfoUI.SetBuildingInfo(constructionRecipe);
    }

    private bool CanBuild()
    {
        foreach (var recipeSlotUI in buildingInfoUI.RecipeSlotUIs)
        {
            if (!recipeSlotUI.gameObject.activeSelf) return true;

            if(!recipeSlotUI.HasEnoughMaterials) return false;
        }
        return true;
    }


}
