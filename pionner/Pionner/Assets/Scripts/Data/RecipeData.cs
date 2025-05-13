using UnityEngine;
using System.Collections.Generic;

public enum RecipeType
{
    None,
    BuildingConstruction, // �ǹ� ������
    ItemCrafting,         // ������ ������ 
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
    public List<RecipeIngredient> ingredients; // �ʿ��� ��� ���

    // ������ �ر� ����
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