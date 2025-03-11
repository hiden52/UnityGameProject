using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManger : MonoBehaviour
{
    public GameObject inventoryUI;


    // Start is called before the first frame update
    void Start()
    {
        PlayerInputManager.Instance.OnTabPressed += ToggleInventoryUI;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ToggleInventoryUI()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }
}
