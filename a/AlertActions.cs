using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertActions : MonoBehaviour
{
    [Header("Coroutine Time")]
    [SerializeField]
    protected float coroutineTime;
    protected WaitForSeconds coroutineDelay;

    [SerializeField] protected Text alertText;
    [SerializeField] protected Image background;
    protected Color textColor;
    protected Color backgroundColor;

    

    protected virtual void Awake()
    {
        if(alertText != null)
            textColor = alertText.color;
        if(background != null)
            backgroundColor = background.color;

        coroutineDelay = new WaitForSeconds(coroutineTime);

    }
    protected void OnEnable()
    {
        StopCoroutine(FadeOutAlert());
        ResetAlpha();


        StartCoroutine(FadeOutAlert());

    }

    protected void ResetAlpha()
    {
        if (textColor != null && backgroundColor != null)
        {
            alertText.color = textColor;
            background.color = backgroundColor;
        }
        else
        {
            return;
        }
    }

    protected virtual IEnumerator FadeOutAlert()
    {
        ResetAlpha();
        Color backColor = background.color;
        Color tColor = alertText.color;
        while (alertText.color.a > 0)
        {

            backColor.a -= 0.001f;
            tColor.a -= 0.01f;

            alertText.color = tColor;
            background.color = backColor;
            yield return null;
        }

        this.gameObject.SetActive(false);
    }

}
