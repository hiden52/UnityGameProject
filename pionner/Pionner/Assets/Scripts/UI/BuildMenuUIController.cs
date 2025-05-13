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

    private void Start()
    {
        InitializeCategoryButtons();
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
                // C#�� Ŭ���� ���� ������ ���� ���� ���� ���
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
            BuildingData buildingDataToShow = constructionRecipe.buildingToConstruct;

            Image iconComponent = buttonGO.gameObject.GetComponentInChildren<Image>();
            Text nameComponent = buttonGO.gameObject.GetComponentInChildren<Text>();
            if (iconComponent != null) iconComponent.sprite = buildingDataToShow.icon;
            if (nameComponent != null) nameComponent.text = buildingDataToShow.BuildingName;


            Button button = buttonGO.GetComponent<Button>();
            if (button != null)
            {
                // C#�� Ŭ���� ���� ������ ���� ���� ���� ���
                BuildingRecipeData currentRecipe = constructionRecipe;
                button.onClick.AddListener(() => SelectBuildingToConstruct(currentRecipe));
            }
        }
    }

    void SelectBuildingToConstruct(BuildingRecipeData constructionRecipe)
    {
        // ���õ� �ǹ�(constructionRecipe.buildingToConstruct)�� �� ������ �����ʿ� ǥ��
        // (�ʿ� ���: constructionRecipe.ingredients)
        // �Ǽ� ��� ���� ���� ���� ȣ��
        Debug.Log($"Selected building to construct: {constructionRecipe.buildingToConstruct.BuildingName}");
        // ��: UIManager.Instance.ShowBuildingInfo(constructionRecipe);
        // ��: PlayerBuildingSystem.Instance.StartPlacementMode(constructionRecipe.buildingToConstruct, constructionRecipe);
    }


}
