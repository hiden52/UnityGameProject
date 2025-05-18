using System.Collections.Generic;
using UnityEngine;

public class CraftRecipeListController : MonoBehaviour
{
    [SerializeField] private Transform recipeListContainer;
    [SerializeField] private CraftRecipeListSlotUI recipeSlotPrefab;

    private List<CraftRecipeListSlotUI> recipeSlots = new List<CraftRecipeListSlotUI>();
    private ItemRecipeForwarder recipeForwarder;

    private void Awake()
    {
        recipeForwarder = GetComponentInParent<ItemRecipeForwarder>();
    }

    public void SetupRecipeList(List<ItemRecipeData> recipes)
    {
        // 기존 슬롯 정리
        ClearRecipeList();

        if (recipes == null || recipes.Count == 0)
            return;

        foreach (var recipe in recipes)
        {
            CreateRecipeSlot(recipe);
        }
    }

    private void CreateRecipeSlot(ItemRecipeData recipe)
    {
        CraftRecipeListSlotUI newSlot = Instantiate(recipeSlotPrefab, recipeListContainer);
        newSlot.Initialize(recipe, OnRecipeSelected);
        recipeSlots.Add(newSlot);
    }

    private void ClearRecipeList()
    {
        foreach (var slot in recipeSlots)
        {
            Destroy(slot.gameObject);
        }
        recipeSlots.Clear();
    }

    private void OnRecipeSelected(ItemRecipeData recipe)
    {
        if (recipeForwarder != null)
        {
            recipeForwarder.SelectRecipe(recipe);
        }
    }
}