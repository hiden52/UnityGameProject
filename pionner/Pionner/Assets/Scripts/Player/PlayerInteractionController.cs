using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] float interactionDistance = 5f;
    [SerializeField] IInteractable currentInteractable;
    private GameObject lastRaycastHitObject = null;

    private void Update()
    {
        bool isHit = false;

        //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 3f, Color.red, 0.1f);
        // 인벤토리UI가 활성화 되어 있을 경우, 레이캐스트 무시
        if (!UIManager.Instance.IsInventoryActivated())
        {
            isHit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, interactionDistance, layerMask);
            if (isHit)
            {
                var newInteractable = hit.collider.GetComponent<IInteractable>();
                GameObject hitObject = hit.collider.gameObject;

                if (hitObject != lastRaycastHitObject)
                {
                    if (lastRaycastHitObject != null)
                    {
                        OutlineManager.Instance.HideOutline();
                    }

                    if (newInteractable != null)
                    {
                        OutlineManager.Instance.ShowOutline(hitObject);
                    }
                    lastRaycastHitObject = hitObject;
                }

                if (newInteractable != null)
                {
                    // InteractionUI 활성화
                    UIManager.Instance.ActivateInteractionUI();
                    // 
                    if (newInteractable != currentInteractable)
                    {
                        //Debug.Log("new Interactable found");
                        if (currentInteractable != null)
                        {
                            //Debug.Log("Delete previous event");
                            PlayerInputManager.Instance.OnKeyFPressed -= currentInteractable.Interact;
                        }
                        PlayerInputManager.Instance.OnKeyFPressed += newInteractable.Interact;
                        currentInteractable = newInteractable;
                    }
                }
            }
            else // IInteractable이 아닌 경우
            {
                if (currentInteractable != null)
                {
                    UIManager.Instance.DeactivateInteractionUI();
                    PlayerInputManager.Instance.OnKeyFPressed -= currentInteractable.Interact;
                    currentInteractable = null;
                }
            }
        }

        if (!isHit)
        {
            if (lastRaycastHitObject != null)
            {
                OutlineManager.Instance.HideOutline();
                lastRaycastHitObject = null;
            }

            if (currentInteractable != null)
            {
                UIManager.Instance.DeactivateInteractionUI();
                PlayerInputManager.Instance.OnKeyFPressed -= currentInteractable.Interact;
                currentInteractable = null;
            }
        }


    }
}
