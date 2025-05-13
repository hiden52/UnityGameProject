using UnityEngine;
using System.Collections.Generic;

public enum RecipeType
{
    None,
    BuildingConstruction, // 건물 레시피
    ItemCrafting,         // 아이템 레시피 
}


public class RecipeData : ScriptableObject
{
    [Header("Basic Info")]
    public string recipeId;
    public string recipeName;
    [TextArea] public string description;
    public Sprite recipeIcon;
    [SerializeField] protected RecipeType recipeType = RecipeType.None;
    public RecipeType RecipeType => recipeType;

    [Header("Ingredients (Inputs)")]
    public List<RecipeIngredient> ingredients; // 필요한 재료 목록

    // 레시피 해금 조건
    // public List<ResearchUnlockCondition> unlockConditions;

    protected void SetRecipeType(RecipeType type)
    {
        recipeType = type;
    }
    protected virtual void OnEnable()
    {

    }
    protected virtual void OnValidate()
    {

    }
}