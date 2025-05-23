using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject equipmentUI;
    [SerializeField] private GameObject buildMenuUI;
    [SerializeField] private GameObject craftUI;

    [SerializeField] private GameObject crosshairUI;
    [SerializeField] private GameObject statusUI;
    [SerializeField] private GameObject gameMenuUI;
    [SerializeField] private GameObject interactionUI;

    private GameObject[] inven;
    private GameObject[] craft;

    [Header("Debug")]
    [SerializeField] private List<GameObject> currentlyActivatedUIs;

    private void Start()
    {
        InitUI();
        PlayerInputManager.Instance.OnTabPressed += ToggleInvens;
        PlayerInputManager.Instance.OnKeyBPressed += ToggleBuildMenuUI;
        PlayerInputManager.Instance.OnEscapePressed += HandleEscapeKey;
        BuildManager.Instance.OnStartBuildMode += ToggleBuildMenuUI;
    }
    protected override void Awake()
    {
        base.Awake();
        currentlyActivatedUIs = new List<GameObject>();
        currentlyActivatedUIs.Capacity = 2;
        inven = new GameObject[2] { inventoryUI, equipmentUI };
        craft = new GameObject[2] { inventoryUI, craftUI };
    }
    private void OnDisable()
    {
        PlayerInputManager.Instance.OnTabPressed -= ToggleInvens;
        PlayerInputManager.Instance.OnKeyBPressed -= ToggleBuildMenuUI;
        PlayerInputManager.Instance.OnEscapePressed -= HandleEscapeKey;
        BuildManager.Instance.OnStartBuildMode -= ToggleBuildMenuUI;
    }

    private void HandleEscapeKey()
    {
        if (currentlyActivatedUIs.Count > 0 )
        {
            if (currentlyActivatedUIs.Contains(gameMenuUI))
            {
                CloseAllWidows();
                gameMenuUI.GetComponent<GameMenuUI>().ResumeGame();
                return;
            }
            else
            {
                CloseAllWidows();
                return;
            }
        }

        gameMenuUI.SetActive(true);
        currentlyActivatedUIs.Add(gameMenuUI);
        crosshairUI.SetActive(false);
        SetMouseState(0);
        gameMenuUI.GetComponent<GameMenuUI>().PauseGame();
    }

    private void ToggleUI(GameObject targetUI)
    {
        if ( currentlyActivatedUIs.Count > 0 )
        {
            foreach (GameObject ui in currentlyActivatedUIs) ui.SetActive(false);

            if (currentlyActivatedUIs.Count == 1 && currentlyActivatedUIs[0] == targetUI)
            {
                crosshairUI.SetActive(true);
                currentlyActivatedUIs.Clear();
                SetMouseState(1);
                return;
            }
        }
        else
        {
            crosshairUI.SetActive(false);
            SetMouseState(0);
        }
        currentlyActivatedUIs.Clear();
        ResetPlayerInputs();
        targetUI.SetActive(true);
        currentlyActivatedUIs.Add(targetUI);
    }

    private void ToggleUI(GameObject[] targetUIs)
    {
        if (currentlyActivatedUIs.Count > 0)
        {
            bool isSameUI = true;
            foreach (var ui in currentlyActivatedUIs.Select((item, index) => (item, index)))
            {
                ui.item.SetActive(false);
                Debug.Log(ui.item.name + " " + ui.index);
                // 순서대로 넣었다는 가정이 있기에 가능
                if (ui.item != targetUIs[ui.index])
                {
                    Debug.Log($"{ui.item.name}({ui.index}) is not {targetUIs[ui.index]}[{ui.index}]");
                    isSameUI = false;
                }
            }
            currentlyActivatedUIs.Clear();

            if (isSameUI)
            {
                crosshairUI.SetActive(true);
                SetMouseState(1);
                return;
            }
        }
        else
        {
            crosshairUI.SetActive(false);
            SetMouseState(0);
        }

        currentlyActivatedUIs.Clear();
        ResetPlayerInputs();
        // 순서대로 넣음
        foreach (var target in targetUIs)
        {
            target.SetActive(true);
            currentlyActivatedUIs.Add(target);
        }
    }
    private void CloseAllWidows()
    {
        if (currentlyActivatedUIs.Count == 0) return;
        foreach (GameObject ui in currentlyActivatedUIs) ui.SetActive(false);
        currentlyActivatedUIs.Clear();
        crosshairUI.SetActive(true);
        SetMouseState(1);
    }
    private void ToggleInvens()
    {
        if (inventoryUI != null && equipmentUI != null)
        {
            ToggleUI(inven);
        }
    }
    private void ToggleInventoryUI()
    {
        if (inventoryUI != null)
        {
            ToggleUI(inventoryUI);
        }
    }
    private void ToggleEquipmentUI()
    {
        if (equipmentUI != null)
        {
            ToggleUI(equipmentUI);
        }
    }
    private void ToggleBuildMenuUI()
    {
        if (buildMenuUI != null)
        {
            ToggleUI(buildMenuUI);
        }
    }
    public void ToggleCraftUI(BuildingType buildingType = BuildingType.None)
    {
        if(craftUI != null)
        {
            ToggleUI(craft);

            ManualCraftSystem manualSystem = craftUI.GetComponent<ManualCraftSystem>();
            AutomatedCraftSystem autoSystem = craftUI.GetComponent<AutomatedCraftSystem>();

            Transform recipeInfo = craftUI.transform.Find("Recipe Info");
            Transform manualPanel = recipeInfo.transform.Find("ManualCraftPanel");
            Transform autoPanel = recipeInfo.transform.Find("AutomatedCraftPanel");
            Debug.Log(manualPanel, manualPanel);
            Debug.Log(autoPanel, autoPanel);
            if (buildingType == BuildingType.Factory)
            {
                if (manualPanel) manualPanel.gameObject.SetActive(false);
                if (autoPanel) autoPanel.gameObject.SetActive(true);

                if (manualSystem) manualSystem.enabled = false;
                if (autoSystem) autoSystem.enabled = true;
            }
            else
            {
                if (manualPanel) manualPanel.gameObject.SetActive(true);
                if (autoPanel) autoPanel.gameObject.SetActive(false);

                if (manualSystem) manualSystem.enabled = true;
                if (autoSystem) autoSystem.enabled = false;
            }
        }
    }

    public void CloseGameMenu()
    {
        if (currentlyActivatedUIs.Contains(gameMenuUI))
        {
            CloseAllWidows();
            gameMenuUI.GetComponent<GameMenuUI>().ResumeGame();
        }
    }

    public bool IsAnyUIActivated()
    {
        return currentlyActivatedUIs.Count > 0;
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
    private void ResetPlayerInputs()
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
        inventoryUI.SetActive(false);
        equipmentUI.SetActive(false);
        buildMenuUI.SetActive(false);
        craftUI.SetActive(false);
        interactionUI.SetActive(false);
    }

}
