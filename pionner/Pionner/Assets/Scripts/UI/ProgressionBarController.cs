using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionBarController : MonoBehaviour
{
    [SerializeField] Image fillment;
    [SerializeField] Text progressionText;
    [SerializeField] float craftTime;
    [SerializeField] float progress;
    [SerializeField] private bool canFill;
    [SerializeField] private ItemRecipeData currentRecipe;

    public event Action<bool> OnCanFillUpdated;
    public event Action<ItemRecipeData> OnCraftingCompleted;
    public float Progress => progress;
    public bool CanFill => canFill;


    private void Awake()
    {
        canFill = false;
        progress = 0;
        if(fillment !=null)
        {
            fillment.fillAmount = progress;
        }
        SetPercentage(progress);
        UpdateBar();
    }
    private IEnumerator StartFill(float time)
    {
        float delta = 0;
        while (canFill)
        {
            if (delta < time)
            {
                progress = Mathf.Lerp(0f, 1f, delta / time);
                delta += Time.deltaTime;
            }
            else
            {
                progress = 1f;
            }
            UpdateBar();

            if (progress >= 1f)
            {
                SetCanFill(false);
                CompletePregress();
            }

            yield return null;
        }

        EndProgress();
    }
    
    public void SetCanFill(bool input)
    {
        if (canFill != input)
        {
            canFill = input;
            OnCanFillUpdated?.Invoke(canFill);
        }
        else
        {
            canFill = input;
        }

    }
    public void StartProgress()
    {
        if (canFill)
        {
            StartCoroutine(StartFill(craftTime));
        }
    }

    private void UpdateFillment()
    {
        fillment.fillAmount = progress;
    }
    private void UpdateText()
    {
        if (progressionText != null)
        {
            progressionText.text = Mathf.Round(progress * 100f) + "%";
        }
    }
    private void CompletePregress()
    {
        if (currentRecipe != null)
        {
            OnCraftingCompleted?.Invoke(currentRecipe);
        }
    }

    private void EndProgress()
    {
        ResetProgression();
    }

    private void UpdateBar()
    {
        UpdateFillment();
        UpdateText();
    }
    private void SetPercentage(float value)
    {
        progress = Mathf.Clamp01(value);
    }

    public void ResetProgression()
    {
        progress = 0;
        UpdateBar();
    }
}
