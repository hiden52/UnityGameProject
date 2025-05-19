using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class PlayerInputManager : Singleton<PlayerInputManager>
{

    [Header("설정")]
    [SerializeField] float mouseSpeed;

    [Header("Debug")]

    [SerializeField] private Vector3 movement;
    public Vector3 Movement { get { return movement; } }
    
    [SerializeField] private Vector2 mousePos;
    public Vector2 MousePos { get { return mousePos; } }
    [SerializeField] bool shift;
    public bool Shift { get { return shift; } }
    [SerializeField] bool mouseLB;
    [SerializeField] bool isJumping;
    [SerializeField] private bool canAttack;
    public bool CanAttack => canAttack;

    public bool IsJumping => isJumping;
    public event Action OnTabPressed;
    public event Action OnLeftMouseClick;
    public event Action OnKeyFPressed;
    public event Action OnAttackPressed;
    public event Action OnKeyBPressed;
    public event Action OnEscapePressed;

    void Start()
    {
        mouseSpeed = 1.0f;
        shift = false;
        mouseLB = false;
        canAttack = true;
    }

    public void SetCanAttack(bool canAttack)
    {
        this.canAttack = canAttack;
    }

    void Update()
    {
        // 인벤토리 비활성화를 위해 Tab 상시 활성화
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OnTabPressed?.Invoke();
        }
        if (Input.GetButtonDown("Build Menu"))
        {
            OnKeyBPressed?.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnEscapePressed?.Invoke();
        }

        // 인벤토리 UI가 활성화 되어 있을 때, 이동, 카메라회전, 캐릭터 상호작용 방지.
        if (UIManager.Instance.IsAnyUIActivated())
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
            if (canAttack == false) return;
            WeaponAttackHandler attackHandler = FindObjectOfType<WeaponAttackHandler>();
            if (attackHandler != null && attackHandler.IsAttacking) return;

            OnLeftMouseClick?.Invoke();
            OnAttackPressed?.Invoke();
        }

        if(Input.GetKeyUp(KeyCode.F))
        {
            OnKeyFPressed?.Invoke();
        }

        if(Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
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
