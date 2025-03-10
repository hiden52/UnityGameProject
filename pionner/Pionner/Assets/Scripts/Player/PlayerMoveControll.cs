using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveControll : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] public Vector3 direction;
    [SerializeField] public Vector3 localDir;
    [SerializeField] float MousX;

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
        localDir = PlayerInputManger.Instance.Movement;
        MousX += PlayerInputManger.Instance.MousePos.x;

        direction = transform.TransformDirection(localDir);
    }

    private void FixedUpdate()
    {
        Rotate();
        Move();
    }

    
    void Move()
    {
        if(PlayerInputManger.Instance.Shift)
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
        transform.localEulerAngles = new Vector3(0, MousX, 0);
    }
}
