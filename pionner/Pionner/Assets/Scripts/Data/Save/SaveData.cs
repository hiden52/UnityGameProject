// SaveData.cs
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    // 플레이어 데이터
    public Vector3Data playerPosition;
    public Vector3Data playerRotation;

    // 인벤토리 데이터
    public List<InventoryItemData> inventory = new List<InventoryItemData>();

    // 건물 데이터
    public List<BuildingSaveData> buildings = new List<BuildingSaveData>();

    // 게임 시간
    public float gameTime;

    // 날짜와 시간
    public string saveDateTime;
}
