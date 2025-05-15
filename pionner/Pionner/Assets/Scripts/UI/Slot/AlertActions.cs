using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertActions : MonoBehaviour
{
    [SerializeField] Text alertText;
    [SerializeField] Image background;
    private Color textColor;
    private Color backgroundColor;

    [Header("Coroutine Time")]
    [SerializeField]
    private float coroutineTime;
    private WaitForSeconds coroutineDelay;

    private void Awake()
    {
        textColor = alertText.color;
        backgroundColor = background.color;
        coroutineDelay = new WaitForSeconds(coroutineTime);

    }
    private void OnDestroy()
    {
        Debug.LogWarning($"AlertActions OnDestroy called for GameObject: Name='{gameObject.name}', InstanceID='{gameObject.GetInstanceID()}'", gameObject);
    }
    private void OnEnable()
    {
        StopCoroutine(FadeOutAlert());
        ResetAlpha();


        StartCoroutine(FadeOutAlert());

    }

    private void ResetAlpha()
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

    private IEnumerator FadeOutAlert()
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
