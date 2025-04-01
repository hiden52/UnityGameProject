using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class PlayerInputManager : Singleton<PlayerInputManager>
{

    [Header("����")]
    [SerializeField] float mouseSpeed;

    [Header("Debug")]

    [SerializeField] private Vector3 movement;
    public Vector3 Movement { get { return movement; } }
    
    [SerializeField] private Vector2 mousePos;
    public Vector2 MousePos { get { return mousePos; } }
    [SerializeField] bool shift;
    public bool Shift { get { return shift; } }
    [SerializeField] bool mouseLB;

    public event Action OnTabPressed;
    public event Action OnLeftMouseClick;
    public event Action OnKeyFPressed;

    void Start()
    {
        mouseSpeed = 1.0f;
        shift = false;
        mouseLB = false;
    }

    void Update()
    {
        // �κ��丮 ��Ȱ��ȭ�� ���� Tab ��� Ȱ��ȭ
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OnTabPressed?.Invoke();
        }

        // �κ��丮 UI�� Ȱ��ȭ �Ǿ� ���� ��, �̵�, ī�޶�ȸ��, ĳ���� ��ȣ�ۿ� ����.
        if (UIManager.Instance.inventoryUI.activeSelf)
        {
            return;
        }
        CheckMousePos();
        CheckMovement();

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            shift = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            shift = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            OnLeftMouseClick?.Invoke();
        }

        if(Input.GetKeyUp(KeyCode.F))
        {
            OnKeyFPressed?.Invoke();
            ClearFKeyEvent();
        }

        

    }

    void CheckMousePos()
    {
        mousePos.x = Input.GetAxis("Mouse X") * mouseSpeed;
        mousePos.y = Input.GetAxis("Mouse Y") * mouseSpeed;
    }

    void CheckMovement()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        movement.Normalize();
    }

    public void HandleMouseLbClick()
    {
        if(mouseLB) mouseLB = false;
    }

    public void ResetMouseDelta()
    {
        mousePos = Vector2.zero;
    }

    public void ResetMovementDelta()
    {
        movement = Vector3.zero;
    }

    public void ClearFKeyEvent()
    {
        OnKeyFPressed = null;
    }



}
