using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AlertManager : Singleton<AlertManager>
{
    private struct AlertData
    {
        public AlertType Type;
        public ItemData ItemData;
        public Sprite itemIcon;
        public int Quantity;
        public string Message;
        public SystemAlertType SystemType;
    }

    private enum AlertType
    {
        Item,
        System,
        Ingredient
    }
    private enum SystemAlertType
    {
        None,
        Save,
        Load,
        Error
    }
    [Header("Alert Prefabs")]
    [SerializeField] private GameObject itemAlertPrefab;
    [SerializeField] private GameObject systemAlertPrefab;
    [SerializeField] private GameObject ingredientAlertPrefab;

    [Header("Alert Settings")]
    [SerializeField] private Transform itemAlertContainer;
    [SerializeField] private Transform ingredientAlertContainer;
    [SerializeField] private Transform systemAlertContainer;
    [SerializeField] private int maxVisibleAlerts = 5;
    [SerializeField] private float alertSpacing = -70f;


    private Queue<GameObject> activeAlerts = new Queue<GameObject>();
    private Queue<AlertData> pendingAlerts = new Queue<AlertData>();

    protected override void Awake()
    {
        base.Awake();

        if (itemAlertPrefab != null)
        {
            ObjectPool.Instance.InitializePool(itemAlertPrefab.name, itemAlertPrefab, 5);
        }

        if (systemAlertPrefab != null)
        {
            ObjectPool.Instance.InitializePool(systemAlertPrefab.name, systemAlertPrefab, 3);
        }

        if (ingredientAlertPrefab != null)
        {
            ObjectPool.Instance.InitializePool(ingredientAlertPrefab.name, ingredientAlertPrefab, 3);
        }
    }

    public void ShowItemObtained(ItemData itemData, int quantity = 1)
    {
        pendingAlerts.Enqueue(new AlertData
        {
            Type = AlertType.Item,
            ItemData = itemData,
            Quantity = quantity
        });

        ProcessAlertQueue();
    }

    public void ShowSaveAlert()
    {
        pendingAlerts.Enqueue(new AlertData
        {
            Type = AlertType.System,
            SystemType = SystemAlertType.Save,
            Message = "게임이 저장되었습니다"
        });

        ProcessAlertQueue();
    }

    public void ShowLoadAlert()
    {
        pendingAlerts.Enqueue(new AlertData
        {
            Type = AlertType.System,
            SystemType = SystemAlertType.Load,
            Message = "게임이 로드되었습니다"
        });

        ProcessAlertQueue();
    }

    public void ShowErrorAlert(string message)
    {
        pendingAlerts.Enqueue(new AlertData
        {
            Type = AlertType.System,
            SystemType = SystemAlertType.Error,
            Message = "오류: " + message
        });

        ProcessAlertQueue();
    }

    public void ShowIngredientAlert(string message, Sprite requireItem)
    {
        pendingAlerts.Enqueue(new AlertData
        {
            Type = AlertType.Ingredient,
            itemIcon = requireItem,
            Message = message
        });

        ProcessAlertQueue();
    }

    private void ProcessAlertQueue()
    {
        if (activeAlerts.Count < maxVisibleAlerts && pendingAlerts.Count > 0)
        {
            AlertData data = pendingAlerts.Dequeue();
            CreateAlert(data);
        }
    }

    private void CreateAlert(AlertData data)
    {
        GameObject alertObj = null;
        Transform alertContainer = null;

        switch (data.Type)
        {
            case AlertType.Item:
                alertObj = ObjectPool.Instance.GetObject(itemAlertPrefab);
                alertContainer = itemAlertContainer;
                break;
            case AlertType.System:
                alertObj = ObjectPool.Instance.GetObject(systemAlertPrefab);
                alertContainer = systemAlertContainer;
                break;
            case AlertType.Ingredient:
                alertObj = ObjectPool.Instance.GetObject(ingredientAlertPrefab);
                alertContainer = ingredientAlertContainer;
                break;
        }

        if (alertObj == null) return;

        alertObj.transform.SetParent(alertContainer, false);
        PositionAlert(alertObj, activeAlerts.Count);
        activeAlerts.Enqueue(alertObj);

        switch (data.Type)
        {
            case AlertType.Item:
                ItemAlertActions itemAlert = alertObj.GetComponent<ItemAlertActions>();
                if (itemAlert != null)
                {
                    itemAlert.ShowItemAlert(data.ItemData, data.Quantity);
                    itemAlert.OnAlertFinished += () => OnAlertFinished(alertObj);
                }
                break;

            case AlertType.System:
                SystemAlertActions systemAlert = alertObj.GetComponent<SystemAlertActions>();
                if (systemAlert != null)
                {
                    switch (data.SystemType)
                    {
                        case SystemAlertType.Save:
                            systemAlert.ShowSaveAlert();
                            break;
                        case SystemAlertType.Load:
                            systemAlert.ShowLoadAlert();
                            break;
                        case SystemAlertType.Error:
                            systemAlert.ShowErrorAlert(data.Message);
                            break;
                    }

                    systemAlert.OnAlertFinished += () => OnAlertFinished(alertObj);
                }
                break;

            case AlertType.Ingredient:
                IngredientAlertActions alert = alertObj.GetComponent<IngredientAlertActions>();
                if (alert != null)
                {
                    alert.ShowIngredientAlert(data.Message, data.itemIcon);
                    alert.OnAlertFinished += () => OnAlertFinished(alertObj);
                }
                break;
        }
        //GameObject alertObj = ObjectPool.Instance.GetObject(itemAlertPrefab);
        //if (alertObj == null) return;

        //alertObj.transform.SetParent(alertContainer, false);
        //PositionAlert(alertObj, activeAlerts.Count);

        //activeAlerts.Enqueue(alertObj);

        //ItemAlertActions alertActions = alertObj.GetComponent<ItemAlertActions>();
        //if (alertActions != null)
        //{
        //    alertActions.ShowItemAlert(data.ItemData, data.Quantity);

        //    alertActions.OnAlertFinished += () => OnAlertFinished(alertObj);
        //}
    }

    private void OnAlertFinished(GameObject alertObj)
    {
        StartCoroutine(HandleAlertFinished(alertObj));
    }

    private IEnumerator HandleAlertFinished(GameObject alertObj)
    {
        yield return new WaitForSeconds(0.1f);

        // 큐에서 제거
        if (activeAlerts.Count > 0 && activeAlerts.Peek() == alertObj)
        {
            activeAlerts.Dequeue();
        }

        ObjectPool.Instance.ReturnObject(alertObj);

        ProcessAlertQueue();

        RepositionAlerts();
    }

    // 알림 위치 설정
    private void PositionAlert(GameObject alertObj, int index)
    {
        RectTransform rectTransform = alertObj.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = new Vector2(0, -index * alertSpacing);
        }
    }

    // 알림 위치 재조정
    private void RepositionAlerts()
    {
        int index = 0;
        foreach (GameObject alert in activeAlerts)
        {
            RectTransform rectTransform = alert.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                StartCoroutine(SmoothRepositionAlert(rectTransform, index));
                index++;
            }
        }
    }

    private IEnumerator SmoothRepositionAlert(RectTransform rectTransform, int index)
    {
        Vector2 startPos = rectTransform.anchoredPosition;
        Vector2 targetPos = new Vector2(0, -index * alertSpacing);
        float duration = 0.3f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        rectTransform.anchoredPosition = targetPos;
    }

    
}