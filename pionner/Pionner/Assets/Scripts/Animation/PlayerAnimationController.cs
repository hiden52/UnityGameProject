using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
    [SerializeField] PlayerMovementController playerMovementController;
    [SerializeField] Vector3 playerDir;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerMovementController = GetComponent<PlayerMovementController>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayerInputManager.Instance.OnLeftMouseClick += HandleAttack;
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
        playerAnimator.SetTrigger("Attack");
    }
}
