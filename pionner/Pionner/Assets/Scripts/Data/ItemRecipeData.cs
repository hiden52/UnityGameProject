using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Recipe", menuName = "Game Data/Recipes/Item Recipe")]
public class ItemRecipeData : RecipeData
{
    [Header("Products (Outputs)")]
    public List<RecipeProduct> products; // 생산되는 아이템 목록

    [Header("Crafting Properties")]
    public float craftingTime = 1f; // 제작에 걸리는 시간

    [Tooltip("This recipe can only be used in buildings of this type. 'None' means manual crafting or not specific to a building type.")]
    public BuildingType requiredBuildingType = BuildingType.None;
    protected override void OnEnable()
    {
        SetRecipeType(RecipeType.ItemCrafting);
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        if (recipeType != RecipeType.ItemCrafting)
        {
            Debug.LogWarning($"'{name}' is a ItemRecipeData. Its RecipeType has been changed in inspector and will be reset to ItemCrafting.", this);

            recipeType = RecipeType.ItemCrafting;
            UnityEditor.EditorUtility.SetDirty(this);
    }

        if (products.Count <= 0)
        {
            Debug.LogWarning($"Item Recipe '{name}' does not have 'Products' assigned.", this);
        }
    }
#endif
}
