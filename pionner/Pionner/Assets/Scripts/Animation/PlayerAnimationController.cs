using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
    [SerializeField] PlayerMovementController playerMovementController;
    [SerializeField] Vector3 playerDir;

    [SerializeField] AnimatorOverrideController armedController;
    [SerializeField] AnimatorOverrideController unarmedController;

    private bool isMoving = false;
    private bool isSprinting = false;
    private bool isArmed = false;
    private bool isGrounded = false;

    private int jumpAnimHash;
    private int moveSpeedHash;
    private int isMovingHash;
    private int isSprintingHash;
    private int isArmedHash;
    private int isGroundedHash;
    private int isAimingHash;
    private int forwardHash;
    private int horizontalInputHash;
    private int verticalInputHash;

    private void Awake()
    {
       
        playerAnimator = GetComponent<Animator>();
        playerMovementController = GetComponentInParent<PlayerMovementController>();
        CacheAnimationParameters();


    }

    void Start()
    {
        PlayerInputManager.Instance.OnLeftMouseClick += HandleAttack;

        // 플레이어 상태 변경 이벤트 구독
        PlayerStateManager.Instance.OnStateChanged += HandleStateChanged;

        // init
        HandleStateChanged(PlayerStateManager.Instance.CurrentState);
        playerMovementController.OnStartJunping += Jump;
    }

    private void OnDestroy()
    {
        PlayerStateManager.Instance.OnStateChanged -= HandleStateChanged;
        playerMovementController.OnStartJunping -= Jump;
    }


    private void CacheAnimationParameters()
    {
        jumpAnimHash = Animator.StringToHash("Jump");
        moveSpeedHash = Animator.StringToHash("MoveSpeed");
        isMovingHash = Animator.StringToHash("IsMoving");
        isSprintingHash = Animator.StringToHash("IsSprinting");
        isArmedHash = Animator.StringToHash("IsArmed");
        isAimingHash = Animator.StringToHash("IsAiming");
        isGroundedHash = Animator.StringToHash("IsGrounded");
        horizontalInputHash = Animator.StringToHash("HorizontalInput");
        verticalInputHash = Animator.StringToHash("VerticalInput");
    }

    private void HandleStateChanged(PlayerState newState)
    {
        switch (newState)
        {
            case PlayerState.Armed:
                if (armedController != null)
                {
                    playerAnimator.runtimeAnimatorController = armedController;
                }
                isArmed = true;
                break;

            case PlayerState.Unarmed:
                if (unarmedController != null)
                {
                    playerAnimator.runtimeAnimatorController = unarmedController;
                }
                isArmed = false;
                break;
        }

        playerAnimator.SetBool(isArmedHash, isArmed);
    }
    
    // Update is called once per frame
    void Update()
    {
        playerDir = playerMovementController.inputDirection;
        isMoving = (playerDir.z != 0 || playerDir.x != 0);
        isSprinting = PlayerInputManager.Instance.Shift;
        isGrounded = playerMovementController.IsGrounded;
        
        
        playerAnimator.SetBool(isMovingHash, isMoving && isGrounded);
        playerAnimator.SetBool(isSprintingHash, isSprinting);
        playerAnimator.SetBool(isGroundedHash, isGrounded);

        
        
    }

    private void Jump()
    {
        isGrounded = false;
        playerAnimator.Play(jumpAnimHash);
        
    }

    private void HandleAttack()
    {
        var weapon = EquipmentManager.Instance.ItemOnHand as WeaponItem;
        if (weapon != null)
        {
            if (weapon.EquipData is WeaponItemData weaponData)
            {
                Debug.Log(weaponData.WeaponType);
                switch (weaponData.WeaponType)
                {
                    case WeaponType.Tool:
                        playerAnimator.SetTrigger("Tool Attack");
                        break;
                    case WeaponType.Sword:
                        playerAnimator.SetTrigger("Sword Attack");
                        break;
                    case WeaponType.Gun:
                        playerAnimator.SetTrigger("Gun Attack");
                        break;
                    default:
                        playerAnimator.SetTrigger("Attack");
                        break;
                }
            }
            else
            {
                playerAnimator.SetTrigger("Attack");
            }
        }
        else
        {
            // 기본(맨손) 공격 - 현재 없음
            // playerAnimator.SetTrigger("Attack");
        }
    }
}
