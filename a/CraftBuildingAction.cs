using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftBuildingAction : MonoBehaviour, IIBuildingActions
{
    [SerializeField] ItemRecipeForwarder recipeForwarder;
    [SerializeField] private BuildingData buildingData;

    private void Awake()
    {
        // ���� GameObject���� BuildingData ���� ��������
        if (buildingData == null)
        {
            BuildingObject buildingObj = GetComponent<BuildingObject>();
            if (buildingObj != null)
            {
                buildingData = buildingObj.BuildingData;
            }
        }
    }

    public void BuildingAction()
    {
        if (recipeForwarder != null && buildingData != null)
        {
            recipeForwarder.SetCurrentBuilding(buildingData);
        }
    }
}
