using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildingSaveData
{
    public int buildingID;
    public Vector3Data position;
    public Vector3Data rotation;
    public bool isActive;

    // 공장 건물일 경우
    public int currentRecipeID = -1;
    public float productionProgress = 0f;

    public BuildingSaveData(int id, Vector3 pos, Vector3 rot, bool active)
    {
        buildingID = id;
        position = new Vector3Data(pos);
        rotation = new Vector3Data(rot);
        isActive = active;
    }
}