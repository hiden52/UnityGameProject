using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionBarController : MonoBehaviour
{
    [SerializeField] Image fillment;
    [SerializeField] Text progressionText;
    [SerializeField] float progress;
    [SerializeField] private bool canFill;

    private void Awake()
    {
        canFill = false;
        progress = 0;
        if(fillment !=null)
        {
            fillment.fillAmount = progress;
        }
        SetPercentage(progress);
    }
    private void Update()
    {
        if (canFill)
        {
            StartCoroutine(StartFill(3f));
        }
    }
    private IEnumerator StartFill(float time)
    {
        float delta = 0;
        while (canFill && delta < time)
        {
            float t = delta / time;
            progress = Mathf.Lerp(0f, 1f, t);
            UpdateBar();

            delta = Time.deltaTime;
            yield return null;
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
            progressionText.text = progress + "%";
        }
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
        UpdateFillment();
        UpdateText();
    }
}
