using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
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
}
