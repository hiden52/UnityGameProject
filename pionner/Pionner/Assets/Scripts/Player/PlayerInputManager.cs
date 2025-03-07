using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManger : MonoBehaviour
{

    [Header("¼³Á¤")]
    [SerializeField] float mouseSpeed;

    [Header("Debug")]
    private static PlayerInputManger instance = null;
    public static PlayerInputManger Instance {  get { return instance; } }

    [SerializeField] private Vector3 movement;
    public Vector3 Movement { get { return movement; } }
    
    [SerializeField] private Vector2 mousePos;
    public Vector2 MousePos { get { return mousePos; } }
    [SerializeField] bool shift;
    public bool Shift { get { return shift; } }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this; 
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        mouseSpeed = 1.0f;
        shift = false;
    }

    void Update()
    {
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

}
