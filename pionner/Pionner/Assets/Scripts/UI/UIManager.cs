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
            // InventoryUI 활성화시 회전값, 이동값 초기화
            PlayerInputManager.Instance.ResetMouseDelta();
            PlayerInputManager.Instance.ResetMovementDelta();
        }
    }
}
