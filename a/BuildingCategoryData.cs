using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Building Category", menuName = "Game Data/Building Category")]
public class BuildingCategoryData : ScriptableObject
{
    public string categoryName;
    public Sprite categoryIcon;
    [TextArea] public string categoryDescription;

    public List<BuildingRecipeData> buildingConstructionRecipes;
}