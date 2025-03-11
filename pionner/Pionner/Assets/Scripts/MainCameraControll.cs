using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainCameraControll : MonoBehaviour
{
    

    [Header("µð¹ö±ë")]
    [SerializeField] GameObject playerHead;
    [SerializeField] Vector2 mouseMovement;
    [SerializeField] float MouseX = 0;
    [SerializeField] float MouseY = 0;


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
        
    }

    void RotateCamera()
    {
        this.transform.localEulerAngles = new Vector3(MouseY, MouseX, 0);
    }
}
