using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActions : MonoBehaviour
{
    [SerializeField] Collider openSite;
    [SerializeField] Animator animator;

    private int nearbyHash;

    private void Awake()
    {
        if (openSite == null)
        {
            Debug.LogError("[DoorActions] Collider opensite is Null");
        }

        animator = GetComponent<Animator>();
        if(animator != null)
        {
            CachingAnimParams();
        }
    }

    private void CachingAnimParams()
    {
        nearbyHash = Animator.StringToHash("character_nearby");
    }

    // 05-08 아직 테스트 안함
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            animator.SetBool(nearbyHash, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        animator.SetBool(nearbyHash, false);
    }
}
