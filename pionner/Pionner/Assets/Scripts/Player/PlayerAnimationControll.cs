using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationControll : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Start running");
            playerAnimator.SetBool("Move", true);
        }

        if(Input.GetKeyUp(KeyCode.W))
        {
            Debug.Log("Stop Moving");
            playerAnimator.SetBool("Move", false);
        }
    }
}
