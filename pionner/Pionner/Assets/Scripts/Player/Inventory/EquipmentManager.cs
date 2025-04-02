using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class EquipmentManager : Singleton<EquipmentManager>
{
    [SerializeField] private GameObject playerRightHand;
    [SerializeField] private Collider playerRightHandCollider;
    private Dictionary<EquipmentWhere, EquipmentItem> equipDictionary = new Dictionary<EquipmentWhere, EquipmentItem>();

    protected override void Awake()
    {
        base.Awake();
        playerRightHandCollider = playerRightHand.GetComponent<Collider>();
    }

    public void Equip(EquipmentItem equipItem)
    {
        equipDictionary.TryGetValue(equipItem.EuipType, out EquipmentItem previousItem);
        if (previousItem != null)
        {
            InventoryManager.Instance.AddItem(previousItem.data);
            equipDictionary.Remove(equipItem.EuipType);
        }
        equipDictionary.Add(equipItem.EuipType, equipItem);

        // 플레이어 손에 아이템 생성해서 장착된 것 처럼 보이게 하기
    }

}
