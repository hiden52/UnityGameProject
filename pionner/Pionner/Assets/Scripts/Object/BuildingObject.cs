using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class BuildingObject : DefaultObject, IInteractable
{
    [SerializeField] private Building building;
    [SerializeField] private List<Animator> animator;
    [SerializeField] bool debug;
    [SerializeField] private BuildingData buildingData;
    private IIBuildingActions buildingActions;

    public BuildingData BuildingData => buildingData;


    private void Awake()
    {
        debug = false;

        if (building == null)
        {
            building = new Building();
        }

        if (animator != null)
        {
            building.SetAnimator(animator);
        }
        buildingActions = GetComponent<IIBuildingActions>();

    }
    public void Interact()
    {
        if (buildingData != null)
        {
            UIManager.Instance.ToggleCraftUI(buildingData.buildingType);
            buildingActions?.BuildingAction();
        }
    }

    private void Update()
    {
        if(debug)
        {
            debug = false;
            if (building != null)
            {
                building.SetBlueprint();
            }
        }
    }
}
