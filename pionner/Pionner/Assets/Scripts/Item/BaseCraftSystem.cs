using System;
using System.Collections;
using UnityEngine;

public abstract class BaseCraftSystem : MonoBehaviour, ICraftSystem
{
    [SerializeField] protected ItemRecipeForwarder recipeForwarder;

    protected bool isCrafting = false;
    protected Coroutine craftingCoroutine;

    public event Action OnCraftingStarted;
    public event Action OnCraftingCancelled;
    public event Action OnCraftingCompleted;

    protected virtual void Awake()
    {
        if (recipeForwarder == null)
            recipeForwarder = GetComponentInParent<ItemRecipeForwarder>();
    }

    protected virtual void OnEnable()
    {
        InventoryManager.Instance.OnItemUpdated += UpdateCraftState;
    }

    protected virtual void OnDisable()
    {
        InventoryManager.Instance.OnItemUpdated -= UpdateCraftState;
        CancelCrafting();
    }

    public virtual bool CanCraft()
    {
        if (recipeForwarder.SelectedRecipe == null)
            return false;

        return HasAllRequiredItems();
    }

    public virtual void StartCrafting()
    {
        if (isCrafting || !CanCraft())
            return;

        isCrafting = true;
        OnCraftingStarted?.Invoke();
    }

    public virtual void CancelCrafting()
    {
        if (!isCrafting)
            return;

        if (craftingCoroutine != null)
        {
            StopCoroutine(craftingCoroutine);
            craftingCoroutine = null;
        }

        isCrafting = false;
        OnCraftingCancelled?.Invoke();
    }

    // 추상 메서드: 구체적인 제작 과정은 서브클래스에서 구현
    public abstract void ProcessCraft();

    // 제작 완료 처리
    protected virtual void CompleteCrafting()
    {
        if (recipeForwarder.SelectedRecipe == null)
            return;

        // 재료 소비
        ConsumeIngredients();

        // 결과물 생성
        CreateProducts();

        // 상태 갱신
        UpdateCraftState();

        // 이벤트 발생
        OnCraftingCompleted?.Invoke();
    }

    protected virtual void ConsumeIngredients()
    {
        foreach (RecipeIngredient ingredient in recipeForwarder.SelectedRecipe.ingredients)
        {
            Debug.Log($"Consume Ingredients: {ingredient.itemData.itemName} ({ingredient.amount})");
            InventoryManager.Instance.ConsumeItemByData(ingredient.itemData, ingredient.amount);
        }
    }

    protected virtual void CreateProducts()
    {
        foreach (RecipeProduct product in recipeForwarder.SelectedRecipe.products)
        {
            InventoryManager.Instance.AddItem(product.itemData, product.amount);
        }
    }

    protected bool HasAllRequiredItems()
    {
        if (recipeForwarder.SelectedRecipe == null)
            return false;

        foreach (RecipeIngredient ingredient in recipeForwarder.SelectedRecipe.ingredients)
        {
            int available = InventoryManager.Instance.GetTotalItemCount(ingredient.itemData);
            if (available < ingredient.amount)
            {
                return false;
            }
        }
        return true;
    }

    // UI 상태 업데이트는 서브클래스에서 구현
    public abstract void UpdateCraftState();
}