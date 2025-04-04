using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class EquipmentManager : Singleton<EquipmentManager>
{
    [SerializeField] private Transform playerRightHandTransform;
    private Dictionary<EquipmentWhere, EquipmentItem> equipDictionary = new Dictionary<EquipmentWhere, EquipmentItem>();

    protected override void Awake()
    {
        base.Awake();
    }
    

    public void Equip(EquipmentItem equipItem)
    {
        if(equipDictionary.TryGetValue(equipItem.EuipType, out EquipmentItem previousItem))
        {
            InventoryManager.Instance.AddItem(previousItem.data);
            equipDictionary.Remove(equipItem.EuipType);

            if(equipItem.EuipType == EquipmentWhere.Hand)
            {
                ObjectPool.Instance.ReturnObject(transform.Find(previousItem.data.prefab.name).gameObject);
            }
        }
        equipDictionary.Add(equipItem.EuipType, equipItem);

        if (equipItem is WeaponItem weaponItem)
        {
            EquipWeapon(weaponItem);
        }



            // �÷��̾� �տ� ������ �����ؼ� ������ �� ó�� ���̰� �ϱ�
        }

        public void EquipWeapon(WeaponItem weaponItem)
    {
        GameObject newWeapon = ObjectPool.Instance.GetObject(weaponItem.data.prefab);
        newWeapon.transform.SetParent(playerRightHandTransform);
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;
    }
}
