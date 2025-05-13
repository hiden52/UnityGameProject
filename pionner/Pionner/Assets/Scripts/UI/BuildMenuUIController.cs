using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuUIController : MonoBehaviour
{
    [Header("Data")]
    [Tooltip("�Ǽ� �޴��� ǥ�õ� �ǹ� ī�װ� ���")]
    public List<BuildingCategoryData> buildingCategories;

    [Header("UI References")]
    public Transform categoryButtonParent; 
    public GameObject categoryButtonPrefab; 

    public Transform buildingListParent; // ���õ� ī�װ��� �ǹ� ����� ǥ�õ� �θ� Transform
    public GameObject buildingButtonPrefab; // �ǹ� ��ư ������ (RecipeData�� ǥ��)

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

        // ���� ��ư�� ���� (���� �����̹Ƿ�)
        foreach (Transform child in categoryButtonParent)
        {
            Destroy(child.gameObject);
        }

        // ī�װ� �����͸�ŭ ��ư ����
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
        // ���õ� �ǹ��� �� ������ Building Info�� ǥ��
        // 
        Debug.Log($"Selected building to construct: {constructionRecipe.buildingToConstruct.BuildingName}");
        
        // �Ǽ� ����
        // BuildManager.Instance.StartPlacementMode(constructionRecipe.buildingToConstruct, constructionRecipe);
    }


}
