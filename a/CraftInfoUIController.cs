using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CraftInfoUIController : MonoBehaviour
{
    [SerializeField] private Text targetItemName;
    [SerializeField] private Image targetIcon;
    [SerializeField] private Transform IngredientsParetn;
    [SerializeField] private RecipeSlotUI[] recipeSlotUIs;

    public RecipeSlotUI[] RecipeSlotUIs => recipeSlotUIs;

    private void Awake()
    {
        targetItemName.text = null;
        recipeSlotUIs = IngredientsParetn.GetComponentsInChildren<RecipeSlotUI>();
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
        Color color = targetIcon.color;
        color.a = alpha;
        targetIcon.color = color;
    }

    private void ResetRecipeSlots()
    {
        foreach (var slot in recipeSlotUIs)
        {
            slot.gameObject.SetActive(false);
        }
    }

    public void ResetInfo()
    {
        ResetRecipeSlots();

        targetItemName.text = null;
        SetIconAlpha(0);
    }

    public void SetBuildingInfo(ItemRecipeData itemRecipe)
    {
        ResetRecipeSlots();
        if(targetItemName == null)
        {
            Debug.LogError("Null this", this);
            return;
        }
        targetItemName.text = itemRecipe.recipeName;
        targetIcon.sprite = itemRecipe.recipeIcon;

        foreach (var ingredient in itemRecipe.ingredients.Select((item, index) => (item, index)))
        {
            recipeSlotUIs[ingredient.index].SetSlot(ingredient.item);
            recipeSlotUIs[ingredient.index].gameObject.SetActive(true);
        }
        SetIconAlpha(1);
    }


}
