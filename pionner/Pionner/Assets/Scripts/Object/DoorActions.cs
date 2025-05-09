using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActions : MonoBehaviour
{
    [SerializeField] Animator animator;

    private int nearbyHash;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError($"[{gameObject.name}.DoorActions] Animator animator is null!");
        }
        CachingAnimParams();
    }

    private void CachingAnimParams()
    {
        nearbyHash = Animator.StringToHash("character_nearby");
    }


    public void HandlePlayerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool(nearbyHash, true);
        }
    }
    public void HandlePlayerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool(nearbyHash, false);
        }
    }
}
