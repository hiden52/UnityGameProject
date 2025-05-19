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

        // ������ ���� Ȯ��
        if (iconImage == null)
        {
            iconImage = transform.Find("SystemIcon")?.GetComponent<Image>();
        }
    }

    // ���� �˸� ǥ��
    public void ShowSaveAlert()
    {
        ShowSystemAlert("������ ����Ǿ����ϴ�", saveIcon);
    }

    // �ε� �˸� ǥ��
    public void ShowLoadAlert()
    {
        ShowSystemAlert("������ �ε�Ǿ����ϴ�", loadIcon);
    }

    // ���� �˸� ǥ��
    public void ShowErrorAlert(string message)
    {
        ShowSystemAlert("����: " + message, errorIcon);
    }

    // �ý��� �˸� ���� ����
    private void ShowSystemAlert(string message, Sprite icon)
    {
        // �ؽ�Ʈ ����
        if (alertText != null)
        {
            alertText.text = message;
        }

        // ������ ����
        if (iconImage != null && icon != null)
        {
            iconImage.sprite = icon;
            iconImage.gameObject.SetActive(true);
        }
        else if (iconImage != null)
        {
            iconImage.gameObject.SetActive(false);
        }

        // �˸� ����
        ResetAlpha();
        StopAllCoroutines();
        StartCoroutine(FadeOutAlert());
    }
}