using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject equipmentsUI;

    [SerializeField] private GameObject crosshairUI;
    [SerializeField] private GameObject statusUI;
    [SerializeField] private GameObject quickMenuUI;
    [SerializeField] private GameObject interactionUI;

    private void Start()
    {
        InitUI();
        PlayerInputManager.Instance.OnTabPressed += ToggleInventoryUI;
    }
    protected override void Awake()
    {
        base.Awake();
    }
    private void OnDisable()
    {
        PlayerInputManager.Instance.OnTabPressed -= ToggleInventoryUI;
    }
        
    void ToggleInventoryUI()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        equipmentsUI.SetActive(!equipmentsUI.activeSelf);
        crosshairUI.SetActive(!crosshairUI.activeSelf);

        if(inventoryUI.activeSelf)
        {
            SetMouseState(0);
            // InventoryUI 활성화시 회전값, 이동값 초기화
            PlayerInputManager.Instance.ResetMouseDelta();
            PlayerInputManager.Instance.ResetMovementDelta();
        }
        else
        {
            SetMouseState(1);
        }
    }

    public bool IsInventoryActivated()
    {
        return inventoryUI.activeSelf;
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

    private void ToggleMouseState()
    {
        Cursor.visible = !Cursor.visible;
        Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void InitUI()
    {
        SetMouseState(1);
        //quickMenuUI.SetActive(true);
        //statusUI.SetActive(true);
        equipmentsUI.SetActive(false);
        crosshairUI.SetActive(true);
        inventoryUI.SetActive(false);
        interactionUI.SetActive(false);
    }

}
