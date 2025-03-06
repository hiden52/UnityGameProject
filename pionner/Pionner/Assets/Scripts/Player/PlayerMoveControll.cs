using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveControll : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] public Vector3 direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        PlayerInput();  
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    void PlayerInput()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.z = Input.GetAxisRaw("Vertical");

        direction.Normalize();
    }
    void PlayerMove()
    {
        rb.MovePosition(rb.position + (direction * speed * Time.fixedDeltaTime));
    }
}
