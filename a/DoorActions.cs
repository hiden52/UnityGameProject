using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActions : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool isDoorCurrentlyOpen = false;
    [SerializeField] private bool playerWasInTrigger = false;

    private int nearbyHash;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError($"[{gameObject.name}.DoorActions] Animator animator is null!");
        }
        isDoorCurrentlyOpen = false;
        playerWasInTrigger= false;
        CachingAnimParams();
    }

    private void CachingAnimParams()
    {
        nearbyHash = Animator.StringToHash("character_nearby");
    }


    public void HandlePlayerEnter(Collider other)
    {
        if (animator == null) return;
        if (other.CompareTag("Player"))
        {
            if (!isDoorCurrentlyOpen)
            {
                Debug.Log($"Player entered {gameObject.name}, opening door.");
                animator.SetBool(nearbyHash, true);
                isDoorCurrentlyOpen = true;
            }
            playerWasInTrigger = true;
        }
    }
    public void HandlePlayerStay(Collider other)
    {
        if (animator == null) return;
        if (other.CompareTag("Player"))
        {
            playerWasInTrigger = true;
            
        }
    }
    public void HandlePlayerExit(Collider other)
    {
        if (animator == null) return;
        if (other.CompareTag("Player"))
        {
            if (isDoorCurrentlyOpen)
            {
                Debug.Log($"Player exited {gameObject.name}, closing door.");
                animator.SetBool(nearbyHash, false);
                isDoorCurrentlyOpen = false;
            }
            playerWasInTrigger = false;
        }
    }

    // 05-09 문사이에 있으면 닫히는 버그
    private void FixedUpdate()
    {
        if (animator == null) return;
        if (!isDoorCurrentlyOpen)
        {
            playerWasInTrigger = false;
            return;
        }

        if (playerWasInTrigger)
        {
            if (!isDoorCurrentlyOpen)
            {
                Debug.Log($"Player still in trigger on {gameObject.name}. Re-open " + gameObject.name);
                animator.SetBool(nearbyHash, true);
                isDoorCurrentlyOpen = true;
            }
        }
        else
        {
            if (isDoorCurrentlyOpen)
            {
                Debug.LogWarning($"FixedUpdate: Player not detected for {gameObject.name} while door was open. Closing door.");
                isDoorCurrentlyOpen = false;
                animator.SetBool(nearbyHash, false);
            }
        }
        playerWasInTrigger = false;

    }
}
