using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    None,
    Normal,
    Factory,
    
}
[CreateAssetMenu(fileName = "New Building Data", menuName = "Building/Building Data")]
public class BuildingData : ScriptableObject
{
    public int BuildingId;
    public string BuildingName;
    [TextArea] public string BuildingDescription;
    public Sprite icon;
    public GameObject prefab;
    public Material originMat;
    public Material bluePrintMat;
    public BuildingType buildingType; 
    public Vector3Int size = Vector3Int.one; //건물이 차지하는 그리드 크기

    [Header("Manufacturing (For Factory-type buildings)")]
    [Tooltip("List of recipes this factory building can process.")]
    public List<RecipeData> availableRecipes; // 이 공장에서 사용할 수 있는 아이템 제작 레시피 목록 (RecipeType.ItemCrafting 이어야 함)

    public bool canRotate = true; 
    public float placementCooldown = 0.5f;
    // 공장 건물의 경우
    public int inputSlotCount;
    public int outputSlotCount;
    public float processingTime; 
}
