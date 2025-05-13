using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buiding Recipe", menuName = "Game Data/Recipes/Buiding Recipe")]
public class BuildingRecipeData : RecipeData

{
    [Header("Products (Outputs)")]
    [Tooltip("The building that will be constructed using this recipe.")]
    public BuildingData buildingToConstruct;

    protected override void OnEnable()
    {
        SetRecipeType(RecipeType.BuildingConstruction);
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        if (recipeType != RecipeType.BuildingConstruction)
        {
            Debug.LogWarning($"'{name}' is a BuildingRecipeData. Its RecipeType has been changed in inspector and will be reset to BuildingConstruction.", this);
            recipeType = RecipeType.BuildingConstruction;
            UnityEditor.EditorUtility.SetDirty(this);
        }

        if (buildingToConstruct == null)
        {
            Debug.LogWarning($"Building Recipe '{name}' does not have 'Building To Construct' assigned.", this);
        }
    }
#endif
}
