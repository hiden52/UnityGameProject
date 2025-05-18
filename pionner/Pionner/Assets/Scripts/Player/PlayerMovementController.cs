using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] Transform mainCamera;
    [SerializeField] Transform followTarget;
    [SerializeField] Rigidbody rb;
    [SerializeField] float moveSpeed;
    [SerializeField] float sprintMultiflier;
    [SerializeField] float rotationSpeed;
    [SerializeField] public Vector3 moveDirection;
    [SerializeField] public Vector3 inputDirection;
    [SerializeField] private LayerMask layerGround;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private Collider footCollider;


    [Header("State")]
    [SerializeField]public bool isAiming = false;
    [SerializeField] private bool isGrounded = true;

    [Header("Debug")]
    [SerializeField] private Collider groundDetector;
    private Collider mainCollider;

    float followYOffset = 1.576f;
    Vector3 followPos;
    private Vector3 forward;
    private Vector3 right;
    private Quaternion targetRotation;

    public bool IsGrounded => isGrounded;

    public event Action OnStartJunping;

    private void Awake()
    {
        mainCollider = GetComponent<Collider>();
        if (mainCollider != null)
        {
            groundDetector = mainCollider;
        }
        rb = GetComponent<Rigidbody>();
        if( footCollider != null )
        {
            footCollider.enabled = false;
        }
        if(rb == null)
        {
            Debug.LogError("[PlayerMovementContoller] RigidBody is null");
        }
    }

    private void Start()
    {
        rb.freezeRotation = true;
        // 05-12 공격할 때, 카메라 중앙 방향으로 캐릭터를 회전시키고 싶음
        // 지금의 방법은 클릭했을 slerp 1번만 실행되면서 카메라 방향으로 완전한 회전이 안됨.
        PlayerInputManager.Instance.OnAttackPressed += RotateTowardsCameraForward;
    }

    private void OnDestroy()
    {
        PlayerInputManager.Instance.OnAttackPressed -= RotateTowardsCameraForward;
    }

    private void Update()
    {
        UpdateCameraDirections();
        if (isGrounded)
        {
            GetInputDirection();
        }
        CalculateMovementDirection();
        
        CheckGround();
        if (PlayerInputManager.Instance.IsJumping && isGrounded)
        {
            Jump();
        }

        followPos = rb.position;
        followPos.y += followYOffset;
        followTarget.position = followPos;
    }

    private void Jump()
    {
        isGrounded = false;
        groundDetector = footCollider;
        footCollider.enabled = true;
        Vector3 jumpPower = Vector3.up * jumpHeight;
        rb.AddForce(jumpPower, ForceMode.VelocityChange);
        OnStartJunping?.Invoke();
    }

    private void FixedUpdate()
    {
        
        Move();
        RotateCharacter();
    }

    private void GetInputDirection()
    {
        Vector3 movementInput = PlayerInputManager.Instance.Movement;
        inputDirection = new Vector3(movementInput.x, 0, movementInput.z);
    }

    private void UpdateCameraDirections()
    {
        forward = mainCamera.forward;
        forward.y = 0;
        forward.Normalize();

        right = mainCamera.right;
        right.y = 0;
        right.Normalize();
    }
    private void CalculateMovementDirection()
    {
        if (inputDirection.magnitude > 0.1f)
        {
            moveDirection = (forward * inputDirection.z + right * inputDirection.x).normalized;
        }
        else
        {
            moveDirection = Vector3.zero;
        }
    }

    private void Move()
    {
        if (moveDirection.magnitude > 0.1f)
        {
            float currentSpeed = moveSpeed;
            if (PlayerInputManager.Instance.Shift)
            {
                currentSpeed *= sprintMultiflier;
            }

            rb.MovePosition(rb.position + moveDirection * currentSpeed * Time.fixedDeltaTime);
        }
    }

    private void RotateCharacter()
    {
        if (isAiming)
        {
            RotateTowardsCameraForward();
        }
        else if (moveDirection.magnitude > 0.1f)
        {
            RotateTowardsMovementDirection();
        }
    }

    // 05-08 Thu.
    // Gorounded 검출을 레이캐스팅으로 하는 중.
    // 콜라이더 충돌 or Trigger 로 검출하면 더 낫지 않을까
    // 생각해보자. 
    // 대신 mainCollider와의 충돌이 발생할 수 있으므로 주의

    //private void OnCollisionEnter(Collision other)
    //{
    //    Debug.Log(gameObject.layer);
    //    if (other.gameObject.layer == layerGround)
    //    {
    //        Debug.Log("Grounded layer");
    //    }
    //}
    private void CheckGround()
    {
        // 콜라이더 사이에 껴서 허공에 계속 떠있는 경우를 방지하기 위해 
        // isGrounded == false 때 mainCollider를 비활성화
        // Bug :: 오브젝트 사이에 아예 끼어버리거나 의도하지 않은 공간으로 들어가버리는 버그가 발생할 수 있음
        // !다른 방법 찾아야함!
        if (Physics.Raycast(groundDetector.transform.position + (Vector3.up * 0.2f), Vector3.down, out RaycastHit hit, 0.6f, layerGround))
        {
            isGrounded = true;
            groundDetector = mainCollider;
            mainCollider.enabled = true;
        }
        else
        {
            Debug.DrawRay(groundDetector.transform.position + (Vector3.up * 0.2f),(Vector3.down * 0.6f), Color.red);
            isGrounded = false;
            groundDetector = footCollider;
            mainCollider.enabled = false;
        }
    }
    private void RotateTowardsMovementDirection()
    {
        targetRotation = Quaternion.LookRotation(forward);

        float currentRotationSpeed = rotationSpeed;
        if (GetComponent<WeaponAttackHandler>()?.CurrentWeapon != null &&
            GetComponent<WeaponAttackHandler>().IsAttacking)
        {
            currentRotationSpeed *= 3.0f;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
            currentRotationSpeed * Time.deltaTime);
    }
    
    
    private void RotateTowardsCameraForward()
    {
        targetRotation = Quaternion.LookRotation(forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }
    
}
