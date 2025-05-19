using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemAlertActions : AlertActions
{
    [SerializeField] protected Image itemIconImage;
    [SerializeField] protected Text quantityText;
    [SerializeField] protected string alertFormat = "{0} 획득!";
    private Color iconAlpha = Color.white;

    public event Action OnAlertFinished;

    protected override void Awake()
    {
        base.Awake();

        if (itemIconImage == null)
        {
            itemIconImage = transform.Find("ItemIcon")?.GetComponent<Image>();
        }

        if (quantityText == null)
        {
            quantityText = transform.Find("QuantityText")?.GetComponent<Text>();
        }
    }

    public virtual void ShowItemAlert(ItemData itemData, int quantity = 1)
    {
        if (itemData == null) return;

        if (itemIconImage != null && itemData.icon != null)
        {
            itemIconImage.sprite = itemData.icon;
            itemIconImage.gameObject.SetActive(true);
        }

        if (alertText != null)
        {
            alertText.text = string.Format(alertFormat, itemData.itemName);
        }

        if (quantityText != null && quantity > 1)
        {
            quantityText.text = "x" + quantity.ToString();
            quantityText.gameObject.SetActive(true);
        }
        else if (quantityText != null)
        {
            quantityText.gameObject.SetActive(false);
        }

        // 초기화 및 페이드 시작
        ResetAlpha();
        itemIconImage.color = iconAlpha;
        StopAllCoroutines();
        StartCoroutine(FadeOutAlert());
    }

    protected override IEnumerator FadeOutAlert()
    {
        yield return StartCoroutine(EntryAnimation());

        yield return new WaitForSeconds(1.5f);

        Color backColor = background.color;
        Color tColor = alertText.color;
        float fadeTime = 0.5f; // 페이드 지속 시간
        float elapsed = 0;

        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeTime;

            backColor.a = Mathf.Lerp(backgroundColor.a, 0, t);
            tColor.a = Mathf.Lerp(textColor.a, 0, t);

            alertText.color = tColor;
            background.color = backColor;

            if (itemIconImage != null)
                itemIconImage.color = new Color(1, 1, 1, 1 - t);

            if (quantityText != null)
                quantityText.color = new Color(quantityText.color.r, quantityText.color.g, quantityText.color.b, 1 - t);

            yield return null;
        }
        OnAlertFinished?.Invoke();
    }

    private IEnumerator EntryAnimation()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 startPos = rectTransform.anchoredPosition;
        Vector2 offset = new Vector2(-50, 0); // 왼쪽에서 나타남
        rectTransform.anchoredPosition = startPos + offset;

        float duration = 0.3f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            rectTransform.anchoredPosition = Vector2.Lerp(startPos + offset, startPos, t);
            yield return null;
        }

        rectTransform.anchoredPosition = startPos;
        yield return null;
    }
}