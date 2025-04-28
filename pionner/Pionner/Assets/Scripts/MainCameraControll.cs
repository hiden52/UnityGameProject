using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainCameraControll : MonoBehaviour
{
    

    [Header("디버깅")]
    [SerializeField] GameObject playerHead;
    [SerializeField] Vector2 mouseMovement;
    [SerializeField] float MouseX = 0;
    [SerializeField] float MouseY = 0;
    [SerializeField] Transform upperBody;


    void Start()
    {
        
    }

    void Update()
    {
        mouseMovement = PlayerInputManager.Instance.MousePos;
        //MouseX += mouseMovement.x;
        MouseY -= mouseMovement.y;
        MouseY = Mathf.Clamp(MouseY, -50f, 50f);

        RotateCamera();
        RotateUpperBody();
    }

    void RotateCamera()
    {
        this.transform.localEulerAngles = new Vector3(MouseY, MouseX, 0);
    }

    void RotateUpperBody()
    {        
        if (upperBody != null)
        {
            upperBody.localEulerAngles = new Vector3(MouseY, 0, 0); // 회전 비율 조정 가능
        }
    }
}
