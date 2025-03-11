using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{

    [Header("¼³Á¤")]
    [SerializeField] float mouseSpeed;

    [Header("Debug")]
    private static PlayerInputManager instance = null;
    public static PlayerInputManager Instance {  get { return instance; } }

    [SerializeField] private Vector3 movement;
    public Vector3 Movement { get { return movement; } }
    
    [SerializeField] private Vector2 mousePos;
    public Vector2 MousePos { get { return mousePos; } }
    [SerializeField] bool shift;
    public bool Shift { get { return shift; } }
    [SerializeField] bool mouseLB;

    public event Action OnTabPressed;
    public event Action OnLeftMouseClick;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this; 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        mouseSpeed = 1.0f;
        shift = false;
        mouseLB = false;
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

        if (Input.GetMouseButtonDown(0))
        {
            OnLeftMouseClick?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OnTabPressed?.Invoke();
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

    public void HandleMouseLbClick()
    {
        if(mouseLB) mouseLB = false;
    }

}
