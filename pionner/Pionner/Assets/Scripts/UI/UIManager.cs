using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public GameObject inventoryUI;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

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

        if(inventoryUI.activeSelf )
        {
            // InventoryUI Ȱ��ȭ�� ȸ����, �̵��� �ʱ�ȭ
            PlayerInputManager.Instance.ResetMouseDelta();
            PlayerInputManager.Instance.ResetMovementDelta();
        }
    }
}
