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
        // 05-12 ������ ��, ī�޶� �߾� �������� ĳ���͸� ȸ����Ű�� ����
        // ������ ����� Ŭ������ slerp 1���� ����Ǹ鼭 ī�޶� �������� ������ ȸ���� �ȵ�.
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
    // Gorounded ������ ����ĳ�������� �ϴ� ��.
    // �ݶ��̴� �浹 or Trigger �� �����ϸ� �� ���� ������
    // �����غ���. 
    // ��� mainCollider���� �浹�� �߻��� �� �����Ƿ� ����

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
        // �ݶ��̴� ���̿� ���� ����� ��� ���ִ� ��츦 �����ϱ� ���� 
        // isGrounded == false �� mainCollider�� ��Ȱ��ȭ
        // Bug :: ������Ʈ ���̿� �ƿ� ��������ų� �ǵ����� ���� �������� �������� ���װ� �߻��� �� ����
        // !�ٸ� ��� ã�ƾ���!
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
