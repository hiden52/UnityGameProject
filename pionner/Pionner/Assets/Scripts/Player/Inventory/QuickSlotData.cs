using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuickSlotData", menuName = "Game Data/Quick Slot Data")]
public class QuickSlotData : ScriptableObject
{
    public List<Item> items = new List<Item>();


}