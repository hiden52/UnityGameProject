using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public GameObject inventoryUI;
    public GameObject crosshairUI;
    public GameObject statusUI;
    public GameObject quickMenuUI;
    public GameObject interactionUI;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        InitUI();
        PlayerInputManager.Instance.OnTabPressed += ToggleInventoryUI;
    }
    private void OnDisable()
    {
        PlayerInputManager.Instance.OnTabPressed -= ToggleInventoryUI;
    }
        
    void ToggleInventoryUI()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        crosshairUI.SetActive(!crosshairUI.activeSelf);
        ToggleMouseState();

        if (inventoryUI.activeSelf )
        {
            // InventoryUI 활성화시 회전값, 이동값 초기화
            PlayerInputManager.Instance.ResetMouseDelta();
            PlayerInputManager.Instance.ResetMovementDelta();
        }
    }

    public void SetStateInteractUI(bool active)
    {
        interactionUI.SetActive(active);
    }

    // int state => 0: Free, 1: Lock
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
        crosshairUI.SetActive(true);
        inventoryUI.SetActive(false);
        interactionUI.SetActive(false);
    }

}
