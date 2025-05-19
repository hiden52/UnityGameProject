using System;
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

    public event Action OnAlertFinished;


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

        yield return new WaitForSeconds(1.5f);

        float fadeTime = 0.5f;
        float elapsed = 0;
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeTime;

            backColor.a = Mathf.Lerp(backgroundColor.a, 0, t);
            tColor.a = Mathf.Lerp(textColor.a, 0, t);

            alertText.color = tColor;
            background.color = backColor;

            yield return null;
        }

        OnAlertFinished?.Invoke();
        gameObject.SetActive(false);
    
    //while (alertText.color.a > 0)
    //{

    //    backColor.a -= 0.001f;
    //    tColor.a -= 0.01f;

    //    alertText.color = tColor;
    //    background.color = backColor;
    //    yield return null;
    //}

    //this.gameObject.SetActive(false);
    }

    public virtual void ShowAlert(string message)
    {
        if (alertText != null)
        {
            alertText.text = message;
        }

        ResetAlpha();
        StopAllCoroutines();
        StartCoroutine(FadeOutAlert());
    }

    public virtual void ShowAlert(string message, ItemData requireItem)
    {
        if (alertText != null)
        {
            alertText.text = message;
        }

        ResetAlpha();
        StopAllCoroutines();
        StartCoroutine(FadeOutAlert());
    }

}
