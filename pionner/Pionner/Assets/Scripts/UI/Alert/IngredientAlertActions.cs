using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientAlertActions : AlertActions
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Sprite itemIcon;


    public void ShowIngredientAlert(string message, Sprite icon)
    {
        if (alertText != null)
        {
            alertText.text = message;
        }

        if (iconImage != null && icon != null)
        {
            itemIcon = icon;
            iconImage.sprite = itemIcon;
            iconImage.gameObject.SetActive(true);
        }
        else if (iconImage != null)
        {
            iconImage.gameObject.SetActive(false);
        }

        ResetAlpha();
        StopAllCoroutines();
        StartCoroutine(FadeOutAlert());
    }
}
