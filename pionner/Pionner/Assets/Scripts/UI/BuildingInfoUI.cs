using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfoUI : MonoBehaviour
{
    [SerializeField] private Text buildingName;
    [SerializeField] private Image buildingIcon;
    [SerializeField] private Text buildingDescription;
    [SerializeField] private Transform buildingRecipeParent;
    [SerializeField] private GameObject slotPrefab;

    private void Start()
    {
        buildingName.text = null;
        buildingDescription.text = null;

    }


}
