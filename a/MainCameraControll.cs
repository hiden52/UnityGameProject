using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainCameraControll : MonoBehaviour
{

    [Header("µð¹ö±ë")]
    [SerializeField] Vector2 mouseMovement;
    [SerializeField] float MouseX = 0;
    [SerializeField] float MouseY = 0;
    [SerializeField] bool isAiming = false;
    [SerializeField] Transform upperBody;

    [Header("¼³Á¤")]
    [SerializeField] float verticalLookSpeed = 1.0f;
    [SerializeField] float horizontalLookSpeed = 1.0f;
    [SerializeField] float verticalClamp = 50f;


    void Start()
    {
        
    }

    void Update()
    {
        mouseMovement = PlayerInputManager.Instance.MousePos;
        MouseX += mouseMovement.x * horizontalLookSpeed;
        MouseY -= mouseMovement.y * verticalLookSpeed;
        MouseY = Mathf.Clamp(MouseY, -verticalClamp, verticalClamp);

        RotateCamera();
        RotateUpperBody();
    }

    void RotateCamera()
    {
        this.transform.localEulerAngles = new Vector3(MouseY, MouseX, 0);
    }

    void RotateUpperBody()
    {        
        if (isAiming && upperBody != null)
        {
            upperBody.localEulerAngles = new Vector3(MouseY, 0, 0); 
        }
    }
}
