using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] public Vector3 direction;  // ���� ���� ����
    [SerializeField] public Vector3 localDir;   // ĳ���� ���� ����
    [SerializeField] float mouseX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.freezeRotation = true;
    }

    private void Update()
    {
        localDir = PlayerInputManager.Instance.Movement;
        mouseX += PlayerInputManager.Instance.MousePos.x;

        direction = transform.TransformDirection(localDir);
    }

    private void FixedUpdate()
    {
        Rotate();
        Move();
    }

    
    void Move()
    {
        if(PlayerInputManager.Instance.Shift)
        {
            rb.MovePosition(rb.position + (direction * (speed + 2f) * Time.fixedDeltaTime));
        }
        else
        {
            rb.MovePosition(rb.position + (direction * speed * Time.fixedDeltaTime));
        }    
        
    }



    void Rotate()
    {
        transform.localEulerAngles = new Vector3(0, mouseX, 0);
    }
}
