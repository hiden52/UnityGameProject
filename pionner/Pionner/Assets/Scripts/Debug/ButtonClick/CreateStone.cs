using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateStone : MonoBehaviour
{
    public Button btn;
    public ResourceItemData target;

    private void Awake()
    {
        btn = GetComponent<Button>();
    }

    private void Start()
    {
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if(target == null)
        {
            Debug.LogWarning("Missing target Item Data!");
            return;
        }
        InventoryManager.Instance.AddItem(target, 8);
    }

}
