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
    public Vector3Int size = Vector3Int.one; //�ǹ��� �����ϴ� �׸��� ũ��

    [Header("Manufacturing (For Factory-type buildings)")]
    [Tooltip("List of recipes this factory building can process.")]
    public List<RecipeData> availableRecipes; // �� ���忡�� ����� �� �ִ� ������ ���� ������ ��� (RecipeType.ItemCrafting �̾�� ��)

    public bool canRotate = true; 
    public float placementCooldown = 0.5f;
    // ���� �ǹ��� ���
    public int inputSlotCount;
    public int outputSlotCount;
    public float processingTime; 
}
