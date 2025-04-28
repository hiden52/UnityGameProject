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



    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerMovementController = GetComponent<PlayerMovementController>();
        
    }

    void Start()
    {
        PlayerInputManager.Instance.OnLeftMouseClick += HandleAttack;

        // 플레이어 상태 변경 이벤트 구독
        PlayerStateManager.Instance.OnStateChanged += HandleStateChanged;

        // init
        HandleStateChanged(PlayerStateManager.Instance.CurrentState);
    }

    private void OnDestroy()
    {
        PlayerStateManager.Instance.OnStateChanged -= HandleStateChanged;
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
                playerAnimator.SetBool("Armed", true);
                break;

            case PlayerState.Unarmed:
                if (unarmedController != null)
                {
                    playerAnimator.runtimeAnimatorController = unarmedController;
                }
                playerAnimator.SetBool("Armed", false);
                break;
        }
    }

    void SetForward()
    {
        playerAnimator.SetBool("Forward", true);
        playerAnimator.SetBool("Backward", false);
    }

    void SetBackward()
    {
        playerAnimator.SetBool("Backward", true);
        playerAnimator.SetBool("Forward", false);
    }

    void SetIdle()
    {
        playerAnimator.SetBool("Move", false);
        playerAnimator.SetBool("Forward", false);
        playerAnimator.SetBool("Backward", false);
    }


    // Update is called once per frame
    void Update()
    {
        playerDir = playerMovementController.localDir;

        if (playerDir.z != 0 || playerDir.x != 0)
        {
            playerAnimator.SetBool("Move", true);
        }
        else
        {
            SetIdle();
        }

        if (playerDir.z > 0)
        {
            SetForward();
        }
        else if (playerDir.z < 0)
        {
            SetBackward();
        }

        if(PlayerInputManager.Instance.Shift)
        {
            playerAnimator.SetBool("Shift", true);
        }
        else
        {
            playerAnimator.SetBool("Shift", false);
        }
        
    }

    private void FixedUpdate() 
    {
        
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
            // 기본 공격 - 현재 없음
            // playerAnimator.SetTrigger("Attack");
        }
    }
}
