// SaveData.cs
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    // �÷��̾� ������
    public Vector3Data playerPosition;
    public Vector3Data playerRotation;

    // �κ��丮 ������
    public List<InventoryItemData> inventory = new List<InventoryItemData>();

    // �ǹ� ������
    public List<BuildingSaveData> buildings = new List<BuildingSaveData>();

    // ���� �ð�
    public float gameTime;

    // ��¥�� �ð�
    public string saveDateTime;
}
