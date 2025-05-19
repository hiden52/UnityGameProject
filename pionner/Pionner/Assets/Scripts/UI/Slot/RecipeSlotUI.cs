using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Security.Cryptography;

public class RecipeSlotUI : SlotUI, IPointerDownHandler
{
    [SerializeField] private SlotUIHoverHandler hoverHandler;
    [SerializeField] private ItemData itemData;

    [Header("Amount UI")]
    [SerializeField] private Slider slider;
    [SerializeField] private int amountHaving;
    [SerializeField] private int amountRequired;
    public bool HasEnoughMaterials => amountHaving >= amountRequired;
    public Sprite Icon => itemData.icon;




    protected override void Awake()
    {
        base.Awake();

        hoverHandler = GetComponent<SlotUIHoverHandler>();
        if (hoverHandler != null)
        {
            hoverHandler.Initialize(backroundImage);
        }
    }
    private void Start()
    {
        UpdateSlotUI();
    }
    private void OnEnable()
    {
        UpdateSlotUI();
        InventoryManager.Instance.OnItemUpdated += UpdateSlotUI;
    }
    private void OnDisable()
    {
        InventoryManager.Instance.OnItemUpdated -= UpdateSlotUI;
    }

    protected void SetQuantity(int n)
    {
        amountRequired = n;
        text.SetActive(true);
        UpdateSlotUI();
    }
    private void UpdateSlotUI ()
    {
        if(!text.activeSelf) text.SetActive(true);
        amountHaving = InventoryManager.Instance.GetTotalItemCount(itemData);
        
        text.GetComponent<Text>().text = amountHaving + "/" + amountRequired;
        SetSlider();
    }
    private void SetSlider()
    {
        if(slider != null)
        {
            slider.enabled = true;
            if (amountRequired > 0)
            {
                slider.value = Mathf.Clamp01((float)amountHaving / amountRequired);
            }
            else
            {
                slider.value = 0;
            }
            
        }
    }

    public void SetSlot(RecipeIngredient _recipeIngredient)
    {
        if (_recipeIngredient.itemData != null)
        {
            itemData = _recipeIngredient.itemData;
            amountRequired = _recipeIngredient.amount;
            icon.sprite = itemData.icon;
            SetAlpha(1f);

            SetQuantity(amountRequired);
            SetSlider();
        }
        else
        {
            ClearSlot();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (true) return;
        // 아직 필요 없음
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log($"{itemData.itemName} Right Button Click!");
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log($"{itemData.itemName} Left Button Click!");
        }

    }
}
