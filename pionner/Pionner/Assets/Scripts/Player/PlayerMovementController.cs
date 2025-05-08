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
    [SerializeField] float rotationSpeed;
    [SerializeField] public Vector3 moveDirection;
    [SerializeField] public Vector3 inputDirection;
    [SerializeField] private LayerMask layerGround;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private Collider footCollider;

    private Collider groundDetector;

    [Header("State")]
    [SerializeField]public bool isAiming = false;
    [SerializeField] private bool isGrounded = true;

    float followYOffset = 1.576f;
    Vector3 followPos;
    private Vector3 forward;
    private Vector3 right;
    private Quaternion targetRotation;

    public bool IsGrounded => isGrounded;

    public event Action OnStartJunping;

    private void Awake()
    {
        groundDetector = GetComponent<Collider>();
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
            groundDetector = footCollider;
            footCollider.enabled = true;
            Vector3 jumpPower = Vector3.up * jumpHeight;
            rb.AddForce(jumpPower, ForceMode.VelocityChange);
            OnStartJunping?.Invoke();
        }

        followPos = rb.position;
        followPos.y += followYOffset;
        followTarget.position = followPos;
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
                currentSpeed *= 1.25f;
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

    private void CheckGround()
    {
        if(Physics.Raycast(groundDetector.transform.position + (Vector3.up * 0.2f), Vector3.down, out RaycastHit hit, 0.6f, layerGround))
        {
            isGrounded = true;
            groundDetector = GetComponent<Collider>();
        }
        else
        {
            isGrounded = false;
            groundDetector = footCollider;
        }
    }
    private void RotateTowardsMovementDirection()
    {
        targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    private void RotateTowardsCameraForward()
    {
        targetRotation = Quaternion.LookRotation(forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }
    
}
