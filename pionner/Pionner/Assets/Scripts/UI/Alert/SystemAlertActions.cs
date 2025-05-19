using UnityEngine;
using UnityEngine.UI;

public class SystemAlertActions : AlertActions
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Sprite saveIcon;
    [SerializeField] private Sprite loadIcon;
    [SerializeField] private Sprite errorIcon;

    protected override void Awake()
    {
        base.Awake();

        // 아이콘 참조 확인
        if (iconImage == null)
        {
            iconImage = transform.Find("SystemIcon")?.GetComponent<Image>();
        }
    }

    // 저장 알림 표시
    public void ShowSaveAlert()
    {
        ShowSystemAlert("게임이 저장되었습니다", saveIcon);
    }

    // 로드 알림 표시
    public void ShowLoadAlert()
    {
        ShowSystemAlert("게임이 로드되었습니다", loadIcon);
    }

    // 오류 알림 표시
    public void ShowErrorAlert(string message)
    {
        ShowSystemAlert("오류: " + message, errorIcon);
    }

    // 시스템 알림 공통 로직
    private void ShowSystemAlert(string message, Sprite icon)
    {
        // 텍스트 설정
        if (alertText != null)
        {
            alertText.text = message;
        }

        // 아이콘 설정
        if (iconImage != null && icon != null)
        {
            iconImage.sprite = icon;
            iconImage.gameObject.SetActive(true);
        }
        else if (iconImage != null)
        {
            iconImage.gameObject.SetActive(false);
        }

        // 알림 시작
        ResetAlpha();
        StopAllCoroutines();
        StartCoroutine(FadeOutAlert());
    }
}