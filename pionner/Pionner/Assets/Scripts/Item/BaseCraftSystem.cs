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

    // �߻� �޼���: ��ü���� ���� ������ ����Ŭ�������� ����
    public abstract void ProcessCraft();

    // ���� �Ϸ� ó��
    protected virtual void CompleteCrafting()
    {
        if (recipeForwarder.SelectedRecipe == null)
            return;

        // ��� �Һ�
        ConsumeIngredients();

        // ����� ����
        CreateProducts();

        // ���� ����
        UpdateCraftState();

        // �̺�Ʈ �߻�
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

    // UI ���� ������Ʈ�� ����Ŭ�������� ����
    public abstract void UpdateCraftState();
}