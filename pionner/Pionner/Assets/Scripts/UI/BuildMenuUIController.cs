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
    public Text buildingInfoName;
    public Image buildingInfoIcon;
    public Text buildingInfoDescription;
    public Transform buildingInfoRecipeTransfrom;
    public GameObject buildingRecipePrefab;

    private void Awake()
    {

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

            GameObject buttonGO = Instantiate(buildingButtonPrefab, buildingListParent);
            BuildingIconSlotUI buildingIconSlot = buttonGO.GetComponent<BuildingIconSlotUI>();
            BuildingData buildingDataToShow = constructionRecipe.buildingToConstruct;
            Debug.Log(buildingDataToShow.BuildingName);
            buildingIconSlot.SetSlot(constructionRecipe);


            Button button = buttonGO.GetComponent<Button>();
            if (button != null)
            {
                BuildingRecipeData currentRecipe = constructionRecipe;
                button.onClick.AddListener(() => SelectBuildingToConstruct(currentRecipe));
            }
        }
    }

    void SelectBuildingToConstruct(BuildingRecipeData constructionRecipe)
    {
        // 선택된 건물의 상세 정보를 Building Info에 표시
        // 
        Debug.Log($"Selected building to construct: {constructionRecipe.buildingToConstruct.BuildingName}");
        
        // 건설 관련
        // BuildManager.Instance.StartPlacementMode(constructionRecipe.buildingToConstruct, constructionRecipe);
    }


}
