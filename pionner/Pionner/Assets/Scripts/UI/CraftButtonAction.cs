using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftButtonAction : MonoBehaviour
{
    [SerializeField] private ProgressionBarController progressBarController;
    [SerializeField] private Button button;
    [SerializeField] private bool isButtonPressed = false;

    public event Action<bool> OnButtonStateChanged;

    private void Awake()
    {
        if (button == null)
        {
            button = GetComponentInChildren<Button>();
        }

        if (button != null)
        {
            AddEventTriggers(button.gameObject);
        }
        else
        {
            Debug.LogError("Button component not found in children", this);
        }
    }

    private void OnEnable()
    {
        if (progressBarController != null)
        {
           // progressBarController.OnCanFillUpdated += SetButtonInteractable;
        }
        if (button != null)
        {
            button.interactable = true;
        }
    }

    private void OnDisable()
    {
        if (progressBarController != null)
        {
            //progressBarController.OnCanFillUpdated -= SetButtonInteractable;
        }
    }

    private void AddEventTriggers(GameObject targetObj)
    {
        EventTrigger existingTrigger = targetObj.GetComponent<EventTrigger>();
        if (existingTrigger != null)
        {
            Destroy(existingTrigger);
        }
        EventTrigger trigger = targetObj.AddComponent<EventTrigger>();

        // 포인터 다운 이벤트
        EventTrigger.Entry pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((data) => { OnButtonDown(); });
        trigger.triggers.Add(pointerDown);

        // 포인터 업 이벤트
        EventTrigger.Entry pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((data) => { OnButtonUp(); });
        trigger.triggers.Add(pointerUp);

        // 포인터 종료 이벤트
        EventTrigger.Entry pointerExit = new EventTrigger.Entry();
        pointerExit.eventID = EventTriggerType.PointerExit;
        pointerExit.callback.AddListener((data) => { OnButtonUp(); });
        trigger.triggers.Add(pointerExit);
    }

    private void OnButtonDown()
    {
        if (button != null && button.interactable)
        {
            isButtonPressed = true;
            OnButtonStateChanged?.Invoke(true);

            if (progressBarController != null && progressBarController.CanFill)
            {
                progressBarController.SetCanFill(true);
                progressBarController.StartProgress();
            }
        }
    }

    private void OnButtonUp()
    {
        if (isButtonPressed)
        {
            isButtonPressed = false;
            OnButtonStateChanged?.Invoke(false);

            if (progressBarController != null)
            {
                progressBarController.SetCanFill(false);
            }
        }
    }

    public void SetButtonInteractable(bool interactable)
    {
        if (button != null)
        {
            button.interactable = interactable;

            if (!interactable && isButtonPressed)
            {
                isButtonPressed = false;
                OnButtonStateChanged?.Invoke(false);

                if (progressBarController != null)
                {
                    progressBarController.SetCanFill(false);
                }
            }
        }
    }
}