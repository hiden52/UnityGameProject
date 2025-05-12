using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject InvenAndEquipmentUI;
    [SerializeField] private GameObject buildMenuUI;

    [SerializeField] private GameObject crosshairUI;
    [SerializeField] private GameObject statusUI;
    [SerializeField] private GameObject quickMenuUI;
    [SerializeField] private GameObject interactionUI;

    [Header("Debug")]
    [SerializeField] private GameObject currentlyActivatedUI;

    private void Start()
    {
        InitUI();
        PlayerInputManager.Instance.OnTabPressed += ToggleInventoryUI;
        PlayerInputManager.Instance.OnKeyBPressed += ToggleBuildMenuUI;
    }
    protected override void Awake()
    {
        base.Awake();
        currentlyActivatedUI = null;
    }
    private void OnDisable()
    {
        PlayerInputManager.Instance.OnTabPressed -= ToggleInventoryUI;
        PlayerInputManager.Instance.OnKeyBPressed -= ToggleBuildMenuUI;
    }
        
    private void ToggleUI(GameObject targetUI)
    {
        if ( currentlyActivatedUI != null )
        {
            currentlyActivatedUI.SetActive(false);

            if (currentlyActivatedUI == targetUI )
            {
                crosshairUI.SetActive(true);
                currentlyActivatedUI = null;
                SetMouseState(1);
                return;
            }
        }
        else
        {
            crosshairUI.SetActive(false);
            SetMouseState(0);
        }

        ResetMouse();
        targetUI.SetActive(true);
        currentlyActivatedUI = targetUI;
    }
    private void ToggleInventoryUI()
    {
        if (InvenAndEquipmentUI != null)
        {
            ToggleUI(InvenAndEquipmentUI);
        }
    }
    private void ToggleBuildMenuUI()
    {
        if (buildMenuUI != null)
        {
            ToggleUI(buildMenuUI);
        }
    }

    public bool IsAnyUIActivated()
    {
        return currentlyActivatedUI != null;
    }
    public void  ActivateInteractionUI()
    {
        interactionUI.SetActive(true);
    }
    public void DeactivateInteractionUI()
    {
        interactionUI.SetActive(false);
    }
    public void SetStateInteractUI(bool active)
    {
        interactionUI.SetActive(active);
    }

    /// <summary>
    /// Set Cursor Visiblity.
    /// 0: visible, 1: Invisible
    /// </summary>
    /// <param name="state">0: visible, 1: Invisible</param>
    public void SetMouseState(int state)
    {
        switch (state)
        {
            case 0:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
            case 1:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            default:
                break;
        }
    }
    private void ResetMouse()
    {
        PlayerInputManager.Instance.ResetMouseDelta();
        PlayerInputManager.Instance.ResetMovementDelta();
    }

    private void ToggleMouseState()
    {
        Cursor.visible = !Cursor.visible;
        Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void InitUI()
    {
        SetMouseState(1);
        //quickMenuUI.SetActive(true);
        statusUI.SetActive(true);
        crosshairUI.SetActive(true);
        InvenAndEquipmentUI.SetActive(false);
        interactionUI.SetActive(false);
    }

}
